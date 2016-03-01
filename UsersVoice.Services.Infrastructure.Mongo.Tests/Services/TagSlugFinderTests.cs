using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Services;
using UsersVoice.Services.Infrastructure.Common.Services;
using Xunit;
using Tag = UsersVoice.Infrastructure.Mongo.Commands.Entities.Tag;

namespace UsersVoice.Services.Infrastructure.Mongo.Tests.Services
{
    [TestClass]
    public class TagSlugFinderTests
    {
        [Fact]
        public void should_throw_ArgumentNullException_if_ISlugGenerator_is_null()
        {
            var mockDbContext = new Mock<ICommandsDbContext>();
            Xunit.Assert.Throws<ArgumentNullException>(() => new TagSlugFinder(null, mockDbContext.Object));
        }

        [Fact]
        public void should_throw_ArgumentNullException_if_ICommandsDbContext_is_null()
        {
            var mockSlugGenerator = new Mock<ISlugGenerator>();
            Xunit.Assert.Throws<ArgumentNullException>(() => new TagSlugFinder(mockSlugGenerator.Object, null));
        }

        [Fact]
        public async Task should_throw_ArgumentNullException_if_tag_is_null()
        {
            var sut = CreateSut(null);

            await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => sut.FindSlugAsync(null));
        }

        [Fact]
        public async Task should_throw_ArgumentException_if_tag_id_is_empty()
        {
            var tag = new Tag()
            {
                Id = Guid.Empty,
                Text = "Lorem Ipsum"
            };

            var sut = CreateSut(null);

            await Xunit.Assert.ThrowsAsync<ArgumentException>(() => sut.FindSlugAsync(tag));
        }

        [Fact]
        public async Task should_throw_ArgumentException_if_tag_text_is_empty()
        {
            var tag = new Tag()
            {
                Id = Guid.NewGuid(),
                Text = String.Empty
            };

            var sut = CreateSut(null);

            await Xunit.Assert.ThrowsAsync<ArgumentException>(() => sut.FindSlugAsync(tag));
        }

        [Fact]
        public async Task should_generate_base_slug_when_no_other_tags_present()
        {
            var tag = new Tag()
            {
                Id = Guid.NewGuid(),
                Text = "Lorem Ipsum"
            };

            var sut = CreateSut(null);

            var slug = await sut.FindSlugAsync(tag);

            slug.ShouldBeEquivalentTo("lorem-ipsum");
        }

        [Fact]
        public async Task should_generate_slug_with_index_when_other_tags_present_with_same_slug()
        {
            var tag = new Tag()
            {
                Id = Guid.NewGuid(),
                Text = "Lorem Ipsum"
            };

            var tags = new[]
            {
                new Tag()
                {
                    Id = Guid.NewGuid(),
                    Text = "Lorem Ipsum",
                    Slug = "lorem-ipsum"
                }
            };
            var sut = CreateSut(tags.ToDictionary(t => t.Id));

            var slug = await sut.FindSlugAsync(tag);

            slug.ShouldBeEquivalentTo("lorem-ipsum-1");
        }

        [Fact]
        public async Task should_generate_slug_with_index_when_other_tags_present_with_different_slug()
        {
            var tag = new Tag()
            {
                Id = Guid.NewGuid(),
                Text = "Lorem Ipsum"
            };

            var tags = new[]
            {
                new Tag()
                {
                    Id = Guid.NewGuid(),
                    Text = "Dolor Amet",
                    Slug = "dolor-amet"
                }
            };
            var sut = CreateSut(tags.ToDictionary(t => t.Id));

            var slug = await sut.FindSlugAsync(tag);

            slug.ShouldBeEquivalentTo("lorem-ipsum");
        }

        private static TagSlugFinder CreateSut(IDictionary<Guid, Tag> tags)
        {
            var mockTagsRepo = RepositoryHelpers.MockRepo(tags);

            var mockDbContext = new Mock<ICommandsDbContext>();
            mockDbContext.SetupGet(c => c.Tags).Returns(mockTagsRepo.Object);

            return new TagSlugFinder(new SlugGenerator(), mockDbContext.Object);
        }

    }
}
