using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Services;
using UsersVoice.Services.Common.CQRS.Commands.Handlers;
using UsersVoice.Services.Infrastructure.Common;
using UsersVoice.Services.TagAPI.CQRS.Commands;
using UsersVoice.Services.TagAPI.CQRS.Events;

namespace UsersVoice.Services.TagAPI.CQRS.Mongo.Commands.Handlers
{
    public class UpdateTagCommandHandler : BaseCommandHandler<UpdateTag>
    {
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;
        private readonly ITagSlugFinder _tagSlugFinder;

        public UpdateTagCommandHandler(ICommandsDbContext commandsDb, IMediator bus, ITagSlugFinder tagSlugFinder, IValidator<UpdateTag> validator)
            : base(validator)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            if (tagSlugFinder == null) throw new ArgumentNullException("tagSlugFinder");
            _commandsDb = commandsDb;
            _bus = bus;
            _tagSlugFinder = tagSlugFinder;
        }

        protected override async Task RunCommand(UpdateTag command)
        {
            var tag = await _commandsDb.Tags.Find(t => t.Id == command.TagId).FirstOrDefaultAsync();
            if (null == tag)
                return;

            tag.Text = command.Text;
            tag.Slug = await _tagSlugFinder.FindSlugAsync(tag);

            await _commandsDb.Tags.UpsertOneAsync(t => t.Id == tag.Id, tag);

            await _bus.PublishAsync(new TagUpserted(tag.Id));
        }
    }
}