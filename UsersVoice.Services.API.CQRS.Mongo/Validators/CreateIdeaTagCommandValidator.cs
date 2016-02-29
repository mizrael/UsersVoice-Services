using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Mongo.Validators
{
    public class CreateUserTagCommandValidator : Validator<CreateUserTag>
    {
        private readonly ICommandsDbContext _db;

        public CreateUserTagCommandValidator(ICommandsDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        protected override async Task RunAsync(CreateUserTag command)
        {
            if (null == command)
            {
                AddError(new ValidationError("command cannot be null"));
                return;
            }

            if (Guid.Empty == command.TagId)
                AddError(new ValidationError("tag id cannot be empty"));
            else if (null == await _db.Tags.Find(i => i.Id == command.TagId).FirstOrDefaultAsync())
                    AddError(new ValidationError("invalid tag id: " + command.TagId));

            if (Guid.Empty == command.UserId)
                AddError(new ValidationError("user id cannot be empty"));
            else if (null == await _db.Users.Find(i => i.Id == command.UserId).FirstOrDefaultAsync())
                AddError(new ValidationError("invalid user id: " + command.UserId));
        }
    }
}
