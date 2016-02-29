using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;
using Assert = Xunit.Assert;
using UsersVoice.Services.Common.CQRS.Commands.Handlers;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.Common.CQRS.Tests.Commands.Handlers
{
    [TestClass]
    public class ValidationCommandHandlerDecoratorTests
    {
        [Fact]
        public void should_throw_ArgumentNullException_if_handler_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ValidationCommandHandlerDecorator<DummyCommand>(null, new DummyValidator()));
        }

        [Fact]
        public async Task should_run_handler_if_validator_is_null()
        {
            var mockHandler = new Mock<IAsyncNotificationHandler<DummyCommand>>(); 
            var sut = new ValidationCommandHandlerDecorator<DummyCommand>(mockHandler.Object, null);

            await sut.Handle(new DummyCommand());

            mockHandler.Verify(h => h.Handle(It.IsAny<DummyCommand>()));
        }

        [Fact]
        public async Task should_run_validator()
        {
            var mockHandler = new Mock<IAsyncNotificationHandler<DummyCommand>>();
            var mockValidator = new Mock<IValidator<DummyCommand>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<DummyCommand>())).Returns(() => Task.FromResult(new ValidationResult(null)));
            var sut = new ValidationCommandHandlerDecorator<DummyCommand>(mockHandler.Object, mockValidator.Object);

            await sut.Handle(new DummyCommand());

            mockValidator.Verify(h => h.ValidateAsync(It.IsAny<DummyCommand>()));
        }

        [Fact]
        public async Task should_throw_ValidationException_if_validator_returns_null()
        {
            var mockHandler = new Mock<IAsyncNotificationHandler<DummyCommand>>();
            var mockValidator = new Mock<IValidator<DummyCommand>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<DummyCommand>())).Returns(() => Task.FromResult((ValidationResult)null));

            var sut = new ValidationCommandHandlerDecorator<DummyCommand>(mockHandler.Object, mockValidator.Object);

            await Assert.ThrowsAsync<ValidationException>(() => sut.Handle(new DummyCommand()));
        }

        [Fact]
        public async Task should_throw_ValidationException_if_validation_fails()
        {
            var validationResult = new ValidationResult(new[] {new ValidationError("lorem")});
            
            var mockHandler = new Mock<IAsyncNotificationHandler<DummyCommand>>();
            var mockValidator = new Mock<IValidator<DummyCommand>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<DummyCommand>())).Returns(() => Task.FromResult(validationResult));
            var sut = new ValidationCommandHandlerDecorator<DummyCommand>(mockHandler.Object, mockValidator.Object);

            Assert.Equal(false, validationResult.Success);
            await Assert.ThrowsAsync<ValidationException>(() => sut.Handle(new DummyCommand()));
        }
    }

    public class DummyCommand : IAsyncNotification
    {
        
    }

    public class DummyValidator : IValidator<DummyCommand>
    {
        public Task<ValidationResult> ValidateAsync(DummyCommand value)
        {
            throw new NotImplementedException();
        }
    }
}
