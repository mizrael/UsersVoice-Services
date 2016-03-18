using System;
using System.Threading.Tasks;
using MediatR;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Services.Common.CQRS.Commands.Handlers;
using UsersVoice.Services.Infrastructure.Common;
using UsersVoice.Services.TagAPI.CQRS.Commands;
using UsersVoice.Services.TagAPI.CQRS.Events;

namespace UsersVoice.Services.TagAPI.CQRS.Mongo.Commands.Handlers
{
    public class CreateIdeaTagCommandHandler : BaseCommandHandler<CreateIdeaTag>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public CreateIdeaTagCommandHandler(ICommandsDbContext commandsDb, IMediator bus, IValidator<CreateIdeaTag> validator)
            : base(validator)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        protected override async Task RunCommand(CreateIdeaTag command)
        {
            var entity = new IdeaTag()
            {
                Id = Guid.NewGuid(),
                IdeaId = command.IdeaId,
                TagId = command.TagId
            };
            await _commandsDb.IdeaTags.UpsertOneAsync(it => it.IdeaId == command.IdeaId && it.TagId == command.TagId, entity);

            await _bus.PublishAsync(new IdeaTagCreated(entity.IdeaId, entity.TagId));
        }
    }
}
