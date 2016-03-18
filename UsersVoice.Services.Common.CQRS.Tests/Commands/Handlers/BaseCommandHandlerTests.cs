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
    public class BaseCommandHandlerTests
    {
        
        [Fact]
        public async Task should_run_validator()
        {
            var mockValidator = new Mock<IValidator<DummyCommand>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<DummyCommand>())).Returns(() => Task.FromResult(new ValidationResult(null)));
            var sut = new DummyCommandHandler(mockValidator.Object);

            await sut.Handle(new DummyCommand());

            mockValidator.Verify(h => h.ValidateAsync(It.IsAny<DummyCommand>()));
        }

        [Fact]
        public async Task should_throw_ValidationException_if_validator_returns_null()
        {
            var mockValidator = new Mock<IValidator<DummyCommand>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<DummyCommand>())).Returns(() => Task.FromResult((ValidationResult)null));

            var sut = new DummyCommandHandler(mockValidator.Object);

            await Assert.ThrowsAsync<ValidationException>(() => sut.Handle(new DummyCommand()));
        }

        [Fact]
        public async Task should_throw_ValidationException_if_validation_fails()
        {
            var validationResult = new ValidationResult(new[] {new ValidationError("lorem")});

            var mockValidator = new Mock<IValidator<DummyCommand>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<DummyCommand>())).Returns(() => Task.FromResult(validationResult));
            var sut = new DummyCommandHandler(mockValidator.Object);

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
            return Task.FromResult(new ValidationResult(null));
        }
    }

    public class DummyCommandHandler : BaseCommandHandler<DummyCommand>
    {
        public DummyCommandHandler(IValidator<DummyCommand> validator) : base(validator)
        {
        }
        protected override Task RunCommand(DummyCommand command)
        {
            return Task.CompletedTask;
        }
    }
}
