using System;
using System.Threading.Tasks;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Validators
{
    public class CreateTagCommandValidator : Validator<CreateTag>
    {
        protected override Task RunAsync(CreateTag command)
        {
            if (null == command)
                AddError(new ValidationError("command cannot be null"));
            if(Guid.Empty == command.TagId)
                AddError(new ValidationError("tag id cannot be empty"));
            if(string.IsNullOrWhiteSpace(command.Text))
                AddError(new ValidationError("tag text cannot be empty"));

            return Task.CompletedTask;
        }
    }
}
