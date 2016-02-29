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
using UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers;
using UsersVoice.Services.API.CQRS.Mongo.Validators;
using UsersVoice.Services.Infrastructure.Common;
using UsersVoice.Services.Infrastructure.Mongo.Tests;
using Xunit;
using Assert = Xunit.Assert;

namespace UsersVoice.Services.API.CQRS.Mongo.Tests.Validators
{
    [TestClass]
    public class VoteIdeaCommandValidatorTests
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
        public async Task should_fail_if_idea_id_is_invalid()
        {
            var sut = CreateSut(null, null);
            var command = new VoteIdea(Guid.NewGuid(), Guid.NewGuid(), 1);

            var result = await sut.ValidateAsync(command);
            result.Success.ShouldBeEquivalentTo(false);
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task should_fail_if_user_id_is_invalid()
        {
            var idea = new Idea()
            {
                Id = Guid.NewGuid()
            };
            var sut = CreateSut(new[] { idea }, null);
            var command = new VoteIdea(idea.Id, Guid.NewGuid(), 1);

            var result = await sut.ValidateAsync(command);
            result.Success.ShouldBeEquivalentTo(false);
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task should_fail_if_user_has_not_enough_points()
        {
            var idea = new Idea()
            {
                Id = Guid.NewGuid()
            };
            var user = new User()
            {
                Id = Guid.NewGuid(),
                AvailablePoints = 1 
            };
            var sut = CreateSut(new[] {idea}, new[] {user});
            var command = new VoteIdea(idea.Id, user.Id, 2);

            var result = await sut.ValidateAsync(command);
            result.Success.ShouldBeEquivalentTo(false);
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task should_fail_if_user_has_already_voted()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                AvailablePoints = 10
            };
            var idea = new Idea()
            {
                Id = Guid.NewGuid()
            };
            idea.Votes.Add(new IdeaVote()
            {
                CreationDate = DateTime.UtcNow,
                VoterId = user.Id,
                Points = 1
            });
            var sut = CreateSut(new[] { idea }, new[] { user });
            var command = new VoteIdea(idea.Id, user.Id, 1);

            var result = await sut.ValidateAsync(command);
            result.Success.ShouldBeEquivalentTo(false);
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task should_be_ok_if_command_is_valid()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                AvailablePoints = 10
            };
            var idea = new Idea()
            {
                Id = Guid.NewGuid()
            };
            var sut = CreateSut(new[] { idea }, new[] { user });
            var command = new VoteIdea(idea.Id, user.Id, 1);

            var result = await sut.ValidateAsync(command);
            result.Success.ShouldBeEquivalentTo(true);
        }

        private static VoteIdeaCommandValidator CreateSut(IEnumerable<Idea> ideas, IEnumerable<User> users)
        {
            var mockIdeasRepo = new Mock<IRepository<Idea>>();
            mockIdeasRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Idea, bool>>>()))
                .Returns(new FakeFindFluent<Idea>(ideas));

            var mockUsersRepo = new Mock<IRepository<User>>();
            mockUsersRepo.Setup(r => r.Find(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new FakeFindFluent<User>(users));

            var mockDbContext = new Mock<ICommandsDbContext>();
            mockDbContext.SetupGet(c => c.Ideas).Returns(mockIdeasRepo.Object);
            mockDbContext.SetupGet(c => c.Users).Returns(mockUsersRepo.Object);

            var sut = new VoteIdeaCommandValidator(mockDbContext.Object);
            return sut;
        }
    }
}
