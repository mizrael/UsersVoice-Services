using System;
using System.Threading.Tasks;
using UsersVoice.Services.Infrastructure.Common;
using UsersVoice.Services.TagAPI.CQRS.Commands;

namespace UsersVoice.Services.TagAPI.CQRS.Validators
{
    public class CreateTagCommandValidator : Validator<CreateTag>
    {
        protected override Task RunAsync(CreateTag command)
        {
            if (null == command)
            {
                AddError(new ValidationError("command cannot be null"));
                return Task.Delay(0);
            }

            if(Guid.Empty == command.TagId)
                AddError(new ValidationError("tag id cannot be empty"));
            if(string.IsNullOrWhiteSpace(command.Text))
                AddError(new ValidationError("tag text cannot be empty"));

            return Task.Delay(0);
        }
    }
}
