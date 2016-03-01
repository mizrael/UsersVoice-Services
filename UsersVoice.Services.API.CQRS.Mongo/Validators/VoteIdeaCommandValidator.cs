using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Mongo.Validators
{
    public class VoteIdeaCommandValidator : Validator<VoteIdea>
    {
        private readonly ICommandsDbContext _commandsDb;

        public VoteIdeaCommandValidator(ICommandsDbContext commandsDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            _commandsDb = commandsDb;
        }

        protected override async Task RunAsync(VoteIdea command)
        {
            var idea = await _commandsDb.Ideas.Find(d => d.Id == command.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                AddError(new ValidationError("invalid idea id: " + command.IdeaId));

            var voter = await _commandsDb.Users.Find(d => d.Id == command.VoterId).FirstOrDefaultAsync();
            if (null == voter)
                AddError(new ValidationError("invalid voter id: " + command.VoterId));
            else if (voter.AvailablePoints < command.Points)
                AddError(new ValidationError("not enough points available for user " + command.VoterId));

            if (null != idea && null != voter)
            {
                var alreadyVoted = idea.Votes.Any(v => v.VoterId == voter.Id);
                if (alreadyVoted)
                    AddError(new ValidationError("cannot vote twice!"));
            }
        }
    }
}
