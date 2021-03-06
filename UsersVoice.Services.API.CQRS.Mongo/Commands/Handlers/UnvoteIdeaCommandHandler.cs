﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Events;
using UsersVoice.Services.Common.CQRS.Commands.Handlers;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers
{
    public class UnvoteIdeaCommandHandler : BaseCommandHandler<UnvoteIdea>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;
        public UnvoteIdeaCommandHandler(ICommandsDbContext commandsDb, IMediator bus, IValidator<UnvoteIdea> validator)
            : base(validator)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        protected override async Task RunCommand(UnvoteIdea command)
        {
            if (null == command)
                throw new ArgumentNullException("command");

            var idea = await _commandsDb.Ideas.Find(d => d.Id == command.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + command.IdeaId);

            var voter = await _commandsDb.Users.Find(d => d.Id == command.VoterId).FirstOrDefaultAsync();
            if (null == voter)
                throw new ArgumentException("invalid voter id: " + command.VoterId);

            var vote = idea.Votes.FirstOrDefault(v => v.VoterId == voter.Id);
            if (null == vote)
                throw new AccessViolationException("user has not voted for this idea");

            idea.Votes.Remove(vote);
            await _commandsDb.Ideas.UpsertOneAsync(u => u.Id == idea.Id, idea);

            voter.AvailablePoints += Math.Abs(vote.Points);
            await _commandsDb.Users.UpsertOneAsync(u => u.Id == voter.Id, voter);

            await _bus.PublishAsync(new IdeaUnvoted(idea.Id, voter.Id));
        }
    }
}
