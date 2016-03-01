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
    public class ApproveIdeaCommandHandler : BaseCommandHandler<ApproveIdea>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public ApproveIdeaCommandHandler(ICommandsDbContext commandsDb, IMediator bus, IValidator<ApproveIdea> validator) : base(validator)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        protected override async Task RunCommand(ApproveIdea command)
        {
            var idea = await _commandsDb.Ideas.Find(d => d.Id == command.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + command.IdeaId);

            if (idea.Status == Idea.IdeaStatus.Approved)
                return;

            idea.Status = Idea.IdeaStatus.Approved;

            await _commandsDb.Ideas.FindOneAndReplaceAsync(d => d.Id == command.IdeaId, idea);

            await _bus.PublishAsync(new IdeaStatusChanged(idea.Id));
        }
    }
}
