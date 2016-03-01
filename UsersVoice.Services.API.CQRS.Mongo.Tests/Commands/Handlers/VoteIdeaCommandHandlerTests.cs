using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Events;
using UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers;
using UsersVoice.Services.Infrastructure.Mongo.Tests;
using Xunit;
using Assert = Xunit.Assert;

namespace UsersVoice.Services.API.CQRS.Mongo.Tests.Commands.Handlers
{
    [TestClass]
    public class VoteIdeaCommandHandlerTests
    {
        private static VoteIdeaCommandHandler CreateSut(IEnumerable<Idea> ideas, IEnumerable<User> users, 
                                                        IMediator mediator = null)
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

            if (null == mediator)
            {
                var singleFactory = new SingleInstanceFactory(type => null);
                var multiFactory = new MultiInstanceFactory(type => Enumerable.Empty<object>());
                mediator = new Mediator(singleFactory, multiFactory);
            }

            var sut = new VoteIdeaCommandHandler(mockDbContext.Object, mediator, null);
            return sut;
        }

        [Fact]
        public async Task should_throw_if_null_command()
        {
            var sut = CreateSut(null, null);
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null));
        }

        [Fact]
        public async Task should_throw_if_invalid_idea_id()
        {
            var sut = CreateSut(null, null);
            var command = new VoteIdea(Guid.NewGuid(), Guid.NewGuid(), 1);
            await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(command));
        }

        [Fact]
        public async Task should_throw_if_invalid_user_id()
        {
            var idea = new Idea()
            {
                Id = Guid.NewGuid()
            };
            var sut = CreateSut(new[] { idea }, null);
            var command = new VoteIdea(idea.Id, Guid.NewGuid(), 1);
            await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(command));
        }

        [Fact]
        public async Task should_add_vote_to_idea()
        {
            var idea = new Idea()
            {
                Id = Guid.NewGuid()
            };
            var user = new User()
            {
                Id = Guid.NewGuid()
            };
            var sut = CreateSut(new[] {idea}, new[] {user});
            var command = new VoteIdea(idea.Id, user.Id, 1);

            await sut.Handle(command);

            idea.Votes.Should().NotBeEmpty();
            var vote = idea.Votes.ElementAt(0);
            vote.Points.ShouldBeEquivalentTo(1);
            vote.VoterId.ShouldBeEquivalentTo(user.Id);
        }

        [Fact]
        public async Task should_decrease_user_points()
        {
            var idea = new Idea()
            {
                Id = Guid.NewGuid()
            };
            var user = new User()
            {
                Id = Guid.NewGuid(),
                AvailablePoints = 10
            };
            var sut = CreateSut(new[] { idea }, new[] { user });
            var command = new VoteIdea(idea.Id, user.Id, 1);

            await sut.Handle(command);
            user.AvailablePoints.ShouldBeEquivalentTo(9);
        }

        [Fact]
        public async Task should_publish_IdeaVoted_event()
        {
            var idea = new Idea()
            {
                Id = Guid.NewGuid()
            };
            var user = new User()
            {
                Id = Guid.NewGuid()
            };

            var mediator = new Mock<IMediator>();
            
            var sut = CreateSut(new[] { idea }, new[] { user }, mediator.Object);
            var command = new VoteIdea(idea.Id, user.Id, 1);

            await sut.Handle(command);

            mediator.Verify(m => m.PublishAsync(It.IsAny<IdeaVoted>()));
        }
    }
}
