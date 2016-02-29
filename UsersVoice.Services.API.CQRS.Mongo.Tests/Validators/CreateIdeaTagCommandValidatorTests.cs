using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Mongo.Validators;
using UsersVoice.Services.Infrastructure.Mongo.Tests;
using Xunit;
namespace UsersVoice.Services.API.CQRS.Mongo.Tests.Validators
{
    [TestClass]
    public class CreateIdeaTagCommandValidatorTests
    {
        [Fact]
        public async Task should_fail_if_command_null()
        {
            var sut = CreateSut(null, null);

            var result = await sut.ValidateAsync(null);
            result.Success.ShouldBeEquivalentTo(false);
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task should_fail_if_tag_id_is_invalid()
        {
            var sut = CreateSut(null, null);
            var command = new CreateIdeaTag(Guid.NewGuid(), Guid.NewGuid());

            var result = await sut.ValidateAsync(command);
            result.Success.ShouldBeEquivalentTo(false);
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task should_fail_if_user_id_is_invalid()
        {
            var tag = new Tag()
            {
                Id = Guid.NewGuid()
            };
            var sut = CreateSut(new[] { tag }, null);
            var command = new CreateIdeaTag(tag.Id, Guid.NewGuid());

            var result = await sut.ValidateAsync(command);
            result.Success.ShouldBeEquivalentTo(false);
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task should_be_ok_if_command_is_valid()
        {
            var idea = new Idea()
            {
                Id = Guid.NewGuid()
            };
            var tag = new Tag()
            {
                Id = Guid.NewGuid()
            };
            var sut = CreateSut(new[] { tag }, new[] { idea });
            var command = new CreateIdeaTag(tag.Id, idea.Id);

            var result = await sut.ValidateAsync(command);
            result.Success.ShouldBeEquivalentTo(true);
            result.Errors.Should().BeEmpty();
        }

        private static CreateIdeaTagCommandValidator CreateSut(IEnumerable<Tag> tags, IEnumerable<Idea> ideas)
        {
            var mockTagsRepo = new Mock<IRepository<Tag>>();
            mockTagsRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Tag, bool>>>()))
                .Returns(new FakeFindFluent<Tag>(tags));

            var mockIdeasRepo = new Mock<IRepository<Idea>>();
            mockIdeasRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Idea, bool>>>()))
                .Returns(new FakeFindFluent<Idea>(ideas));

            var mockDbContext = new Mock<ICommandsDbContext>();
            mockDbContext.SetupGet(c => c.Tags).Returns(mockTagsRepo.Object);
            mockDbContext.SetupGet(c => c.Ideas).Returns(mockIdeasRepo.Object);

            var sut = new CreateIdeaTagCommandValidator(mockDbContext.Object);
            return sut;
        }
    }
}
