using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Infrastructure.Mongo.Services;
using UsersVoice.Services.Infrastructure.Common.Services;
using UsersVoice.Services.Infrastructure.Mongo.Tests;
using UsersVoice.Services.TagAPI.CQRS.Commands;
using UsersVoice.Services.TagAPI.CQRS.Events;
using UsersVoice.Services.TagAPI.CQRS.Mongo.Commands.Handlers;
using Xunit;

namespace UsersVoice.Services.TagAPI.CQRS.Mongo.Tests.Commands.Handlers
{
    [TestClass]
    public class CreateTagCommandHandlerTests
    {
        [Fact]
        public async Task should_insert_new_tag()
        {
            var tags = new Dictionary<Guid, Tag>();
            var command = new CreateTag(Guid.NewGuid(), "Lorem Ipsum");

            var sut = CreateSut(tags);
            await sut.Handle(command);

            tags.Count.ShouldBeEquivalentTo(1);
            tags.ContainsKey(command.TagId).ShouldBeEquivalentTo(true);

            tags[command.TagId].Slug.ShouldBeEquivalentTo("lorem-ipsum");
        }

        [Fact]
        public async Task should_publish_TagCreated_event()
        {
            var command = new CreateTag(Guid.NewGuid(), "Lorem Ipsum");

            var mediator = new Mock<IMediator>();

            var sut = CreateSut(null, mediator.Object);

            await sut.Handle(command);

            mediator.Verify(m => m.PublishAsync(It.IsAny<TagUpserted>()));
        }

        private static CreateTagCommandHandler CreateSut(IDictionary<Guid, Tag> tags, IMediator mediator = null)
        {
            var mockTagsRepo = RepositoryHelpers.MockCommandsRepo(tags);

            var mockDbContext = new Mock<ICommandsDbContext>();
            mockDbContext.SetupGet(c => c.Tags).Returns(mockTagsRepo.Object);

            if (null == mediator)
            {
                var singleFactory = new SingleInstanceFactory(type => null);
                var multiFactory = new MultiInstanceFactory(type => Enumerable.Empty<object>());
                mediator = new Mediator(singleFactory, multiFactory);
            }

            var slugFinder = new TagSlugFinder(new SlugGenerator(45), mockDbContext.Object);

            var sut = new CreateTagCommandHandler(mockDbContext.Object, mediator, slugFinder, null);
            return sut;
        }
    }
}
