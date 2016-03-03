using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Events;
using UsersVoice.Services.Common.CQRS.Commands.Handlers;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers
{
    public class VoteIdeaCommandHandler : BaseCommandHandler<VoteIdea>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public VoteIdeaCommandHandler(ICommandsDbContext commandsDb, IMediator bus, IValidator<VoteIdea> validator)
            : base(validator)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        protected override async Task RunCommand(VoteIdea command)
        {
            if (null == command)
                throw new ArgumentNullException("command");

            var idea = await _commandsDb.Ideas.Find(d => d.Id == command.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + command.IdeaId);

            var voter = await _commandsDb.Users.Find(d => d.Id == command.VoterId).FirstOrDefaultAsync();
            if (null == voter)
                throw new ArgumentException("invalid voter id: " + command.VoterId);

            idea.Votes.Add(new IdeaVote()
            {
                CreationDate = DateTime.Now,
                VoterId = voter.Id,
                Points = command.Points
            });

            await _commandsDb.Ideas.UpsertOneAsync(d => d.Id == command.IdeaId, idea);

            voter.AvailablePoints -= Math.Abs(command.Points);
            await _commandsDb.Users.UpsertOneAsync(u => u.Id == voter.Id, voter);

            await _bus.PublishAsync(new IdeaVoted(idea.Id, voter.Id));
        }
    }
}
