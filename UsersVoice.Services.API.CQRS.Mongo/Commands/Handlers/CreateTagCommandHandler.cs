using System;
using System.Threading.Tasks;
using MediatR;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Events;
using Tag = UsersVoice.Infrastructure.Mongo.Commands.Entities.Tag;

namespace UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers
{
    public class CreateTagCommandHandler : IAsyncNotificationHandler<CreateTag>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public CreateTagCommandHandler(ICommandsDbContext commandsDb, IMediator bus)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        public async Task Handle(CreateTag command)
        {
            var newTag = new Tag()
            {
                Id = command.TagId,
                Text = command.Text
            };
            await _commandsDb.Tags.InsertOneAsync(newTag);

            await _bus.PublishAsync(new TagCreated(newTag.Id));
        }
    }
}
