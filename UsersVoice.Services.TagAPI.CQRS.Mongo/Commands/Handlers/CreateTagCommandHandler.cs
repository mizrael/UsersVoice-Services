using System;
using System.Threading.Tasks;
using MediatR;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Services;
using UsersVoice.Services.Common.CQRS.Commands.Handlers;
using UsersVoice.Services.Infrastructure.Common;
using UsersVoice.Services.TagAPI.CQRS.Commands;
using UsersVoice.Services.TagAPI.CQRS.Events;
using Tag = UsersVoice.Infrastructure.Mongo.Commands.Entities.Tag;

namespace UsersVoice.Services.TagAPI.CQRS.Mongo.Commands.Handlers
{
    public class CreateTagCommandHandler : BaseCommandHandler<CreateTag>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;
        private readonly ITagSlugFinder _tagSlugFinder;

        public CreateTagCommandHandler(ICommandsDbContext commandsDb, IMediator bus, ITagSlugFinder tagSlugFinder, IValidator<CreateTag> validator)
            : base(validator)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            if (tagSlugFinder == null) throw new ArgumentNullException("tagSlugFinder");
            _commandsDb = commandsDb;
            _bus = bus;
            _tagSlugFinder = tagSlugFinder;
        }

        protected override async Task RunCommand(CreateTag command)
        {
            var newTag = new Tag()
            {
                Id = command.TagId,
                Text = command.Text
            };

            newTag.Slug = await _tagSlugFinder.FindSlugAsync(newTag);

            await _commandsDb.Tags.InsertOneAsync(newTag);

            await _bus.PublishAsync(new TagUpserted(newTag.Id));
        }
    }
}
