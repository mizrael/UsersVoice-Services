using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Mongo.Validators
{
    public class ApproveIdeaCommandValidator : Validator<ApproveIdea>
    {
        private readonly ICommandsDbContext _commandsDb;

        public ApproveIdeaCommandValidator(ICommandsDbContext commandsDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            _commandsDb = commandsDb;
        }

        protected override async Task RunAsync(ApproveIdea command)
        {
            var idea = await _commandsDb.Ideas.Find(d => d.Id == command.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                AddError(new ValidationError("invalid idea id: " + command.IdeaId));
            else if (idea.Status != Idea.IdeaStatus.Nothing)
                AddError(new ValidationError("invalid status"));

            var user = await _commandsDb.Users.Find(d => d.Id == command.UserId).FirstOrDefaultAsync();
            if (null == user)
                AddError(new ValidationError("invalid user id: " + command.UserId));
            else if (!user.IsAdmin)
                AddError(new ValidationError("user is not authorized to execute this action"));
        }
    }
}
