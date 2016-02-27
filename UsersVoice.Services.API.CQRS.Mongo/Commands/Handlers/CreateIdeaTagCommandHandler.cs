﻿using System;
using System.Threading.Tasks;
using MediatR;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Events;

namespace UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers
{
    public class CreateIdeaTagCommandHandler : IAsyncNotificationHandler<CreateIdeaTag>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public CreateIdeaTagCommandHandler(ICommandsDbContext commandsDb, IMediator bus)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        public async Task Handle(CreateIdeaTag command)
        {
            var entity = new IdeaTag()
            {
                Id = Guid.NewGuid(),
                IdeaId = command.IdeaId,
                TagId = command.TagId
            };
            await _commandsDb.IdeaTags.InsertOneAsync(entity);

            await _bus.PublishAsync(new IdeaTagCreated(entity.IdeaId, entity.TagId));
        }
    }
}