using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Validators;
using Xunit;

namespace UsersVoice.Services.API.CQRS.Tests.Validators
{
    [TestClass]
    public class CreateTagCommandValidatorTests 
    {
        [Fact]
        public async Task should_fail_if_command_null()
        {
            var sut = new CreateTagCommandValidator();

            var result = await sut.ValidateAsync(null);

            result.Success.ShouldBeEquivalentTo(false);
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task should_fail_if_tag_id_is_empty()
        {
            var command = new CreateTag(Guid.Empty, "lorem");

            var sut = new CreateTagCommandValidator();

            var result = await sut.ValidateAsync(command);

            result.Success.ShouldBeEquivalentTo(false);
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task should_fail_if_text_is_empty()
        {
            var command = new CreateTag(Guid.NewGuid(), "");

            var sut = new CreateTagCommandValidator();

            var result = await sut.ValidateAsync(command);

            result.Success.ShouldBeEquivalentTo(false);
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task should_be_ok_if_command_is_valid()
        {
            var command = new CreateTag(Guid.NewGuid(), "lorem");

            var sut = new CreateTagCommandValidator();

            var result = await sut.ValidateAsync(command);

            result.Success.ShouldBeEquivalentTo(true);
            result.Errors.Should().BeEmpty();
        }
    }
}
