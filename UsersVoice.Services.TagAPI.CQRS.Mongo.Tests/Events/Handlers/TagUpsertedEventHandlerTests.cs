using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.Infrastructure.Mongo.Tests;
using UsersVoice.Services.TagAPI.CQRS.Events;
using UsersVoice.Services.TagAPI.CQRS.Mongo.Events.Handlers;
using Xunit;
using QueryTag = UsersVoice.Infrastructure.Mongo.Queries.Entities.Tag;

namespace UsersVoice.Services.TagAPI.CQRS.Mongo.Tests.Events.Handlers
{
    [TestClass]
    public class TagUpsertedEventHandlerTests
    {
        [Fact]
        public async Task should_throw_ArgumentNullException_if_event_is_null()
        {
            var sut = CreateSut(null, null);
            await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null));
        }
        
        [Fact]
        public async Task should_throw_ArgumentException_if_command_tag_does_not_exist()
        {
            var @event = new TagUpserted(Guid.NewGuid());

            var sut = CreateSut(null, null);
            await Xunit.Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(@event));
        }

        [Fact]
        public async Task should_create_new_query_tag_if_not_existing()
        {
            var tag = new Tag()
            {
                Id = Guid.NewGuid(),
                Text = "Lorem Ipsum"
            };

            var tags = new [] { tag }.ToDictionary(t => t.Id);
            var queryTags = new List<QueryTag>();
            var @event = new TagUpserted(tag.Id);

            var sut = CreateSut(tags, queryTags);
            await sut.Handle(@event);

            queryTags.Count.ShouldBeEquivalentTo(1);
            queryTags[0].Id.ShouldBeEquivalentTo(tag.Id);
            queryTags[0].Text.ShouldBeEquivalentTo(tag.Text);
            queryTags[0].Slug.ShouldBeEquivalentTo(tag.Slug);
        }

        [Fact]
        public async Task should_update_query_tag_if_existing()
        {
            var tag = new Tag()
            {
                Id = Guid.NewGuid(),
                Text = "Lorem ipsum"
            };

            var tags = new[] { tag }.ToDictionary(t => t.Id);

            var queryTags = new List<QueryTag>()
            {
                new QueryTag()
                {
                    Id = tag.Id,
                    Text = "asdasdasdasd",
                    Slug = "aaaaaaaaa"
                }
            };

            var @event = new TagUpserted(tag.Id);

            var sut = CreateSut(tags, queryTags, t => t.Id == tag.Id);
            await sut.Handle(@event);

            queryTags.Count.ShouldBeEquivalentTo(1);
            queryTags[0].Id.ShouldBeEquivalentTo(tag.Id);
            queryTags[0].Text.ShouldBeEquivalentTo(tag.Text);
            queryTags[0].Slug.ShouldBeEquivalentTo(tag.Slug);
        }

        private static TagUpsertedEventHandler CreateSut(IDictionary<Guid, Tag> commandTags, IList<QueryTag> queryTags, Func<QueryTag, bool> queryTagFinder = null)
        {
            var mockCommandsTagRepo = RepositoryHelpers.MockCommandsRepo(commandTags);

            var mockCommandsDbContext = new Mock<ICommandsDbContext>();
            mockCommandsDbContext.SetupGet(c => c.Tags).Returns(mockCommandsTagRepo.Object);

            var mockQueriesTagRepo = RepositoryHelpers.MockQueriesRepo(queryTags, queryTagFinder);

            var mockQueryDbContext = new Mock<IQueriesDbContext>();
            mockQueryDbContext.SetupGet(c => c.Tags).Returns(mockQueriesTagRepo.Object);

            var sut = new TagUpsertedEventHandler(mockCommandsDbContext.Object, mockQueryDbContext.Object);
            return sut;
        }
    }
}