using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Events.Handlers
{
    public class TagCreatedEventHandler : IAsyncNotificationHandler<CQRS.Events.TagCreated>
    {
        private readonly ICommandsDbContext _commandsDb;
        private readonly IQueriesDbContext _queryDb;

        public TagCreatedEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _commandsDb = commandsDb;
            _queryDb = queryDb;
        }

        public async Task Handle(CQRS.Events.TagCreated @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var tag = await _commandsDb.Tags.Find(d => d.Id == @event.TagId).FirstOrDefaultAsync();
            if (null == tag)
                throw new ArgumentException("invalid tag id: " + @event.TagId);

            var newTag = new UsersVoice.Infrastructure.Mongo.Queries.Entities.Tag()
            {
                 Text = tag.Text,
                 Id = tag.Id,
                 Slug = tag.Slug
            };
            await _queryDb.Tags.InsertOneAsync(newTag);
        }
    }
}
