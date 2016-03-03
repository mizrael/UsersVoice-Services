using System;
using System.Threading.Tasks;
using MediatR;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Events;
using UsersVoice.Services.Common.CQRS.Commands.Handlers;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers
{
    public class CreateUserTagCommandHandler : BaseCommandHandler<CreateUserTag>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public CreateUserTagCommandHandler(ICommandsDbContext commandsDb, IMediator bus, IValidator<CreateUserTag> validator)
            : base(validator)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        protected override async Task RunCommand(CreateUserTag command)
        {
            var entity = new UserTag()
            {
                Id = Guid.NewGuid(),
                UserId = command.UserId,
                TagId = command.TagId
            };
            await _commandsDb.UserTags.UpsertOneAsync(it => it.UserId == command.UserId && it.TagId == command.TagId, entity);
            
            await _bus.PublishAsync(new UserTagCreated(entity.UserId, entity.TagId));
        }
    }
}
