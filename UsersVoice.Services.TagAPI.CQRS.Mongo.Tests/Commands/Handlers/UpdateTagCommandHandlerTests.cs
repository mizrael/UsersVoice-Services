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
    public class UpdateTagCommandHandlerTests
    {
        [Fact]
        public async Task should_do_nothing_if_tag_does_not_exist()
        {
            var tags = new Tag[] {}.ToDictionary(t => t.Id);

            var command = new UpdateTag(Guid.NewGuid(), "Lorem Ipsum");

            var sut = CreateSut(tags);
            await sut.Handle(command);

            tags.Count.ShouldBeEquivalentTo(0);
        }

        [Fact]
        public async Task should_update_tag()
        {
            var tag = new Tag()
            {
                Id = Guid.NewGuid(),
                Text = "Lorem"
            };
            var tags = new Tag[]
            {
                tag
            }.ToDictionary(t => t.Id);

            var command = new UpdateTag(tag.Id, "Lorem Ipsum");

            var sut = CreateSut(tags);

            await sut.Handle(command);

            tag.Text.ShouldBeEquivalentTo("Lorem Ipsum");
            tag.Slug.ShouldBeEquivalentTo("lorem-ipsum");
        }

        [Fact]
        public async Task should_publish_TagUpserted_event()
        {
            var tag = new Tag()
            {
                Id = Guid.NewGuid(),
                Text = "Lorem"
            };
            var tags = new Tag[]
            {
                tag
            }.ToDictionary(t => t.Id);

            var command = new UpdateTag(tag.Id, "Lorem Ipsum");

            var mediator = new Mock<IMediator>();

            var sut = CreateSut(tags, mediator.Object);

            await sut.Handle(command);

            mediator.Verify(m => m.PublishAsync(It.IsAny<TagUpserted>()));
        }

        private static UpdateTagCommandHandler CreateSut(IDictionary<Guid, Tag> tags, IMediator mediator = null)
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

            var sut = new UpdateTagCommandHandler(mockDbContext.Object, mediator, slugFinder, null);
            return sut;
        }
    }
}
