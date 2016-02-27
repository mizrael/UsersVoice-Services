using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.Infrastructure.Common.Validation;

namespace UsersVoice.Services.API.CQRS.Mongo.Validators
{
    public class CreateIdeaTagCommandValidator : Validator<CreateIdeaTag>
    {
        private readonly ICommandsDbContext _db;

        public CreateIdeaTagCommandValidator(ICommandsDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        protected override async Task RunAsync(CreateIdeaTag command)
        {
            if (null == command)
                AddError(new ValidationError("command cannot be null"));

            if (Guid.Empty == command.TagId)
                AddError(new ValidationError("tag id cannot be empty"));

            if (Guid.Empty == command.IdeaId)
                AddError(new ValidationError("idea id cannot be empty"));

            if(null == await _db.Ideas.Find(i => i.Id == command.IdeaId).FirstOrDefaultAsync())
                AddError(new ValidationError("invalid idea id: " + command.IdeaId));

            if (null == await _db.Tags.Find(i => i.Id == command.TagId).FirstOrDefaultAsync())
                AddError(new ValidationError("invalid tag id: " + command.TagId));
        }
    }
}
