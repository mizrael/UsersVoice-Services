using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.TagAPI.CQRS.Events;

namespace UsersVoice.Services.TagAPI.CQRS.Mongo.Events.Handlers
{
    public class TagUpsertedEventHandler : IAsyncNotificationHandler<TagUpserted>
    {
        private readonly ICommandsDbContext _commandsDb;
        private readonly IQueriesDbContext _queryDb;

        public TagUpsertedEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _commandsDb = commandsDb;
            _queryDb = queryDb;
        }

        public async Task Handle(TagUpserted @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var srcTag = await _commandsDb.Tags.Find(d => d.Id == @event.TagId).FirstOrDefaultAsync();
            if (null == srcTag)
                throw new ArgumentException("invalid tag id: " + @event.TagId);

            var destTag = new UsersVoice.Infrastructure.Mongo.Queries.Entities.Tag()
            {
                Text = srcTag.Text,
                Id = srcTag.Id,
                Slug = srcTag.Slug
            };
            await _queryDb.Tags.UpsertOneAsync(t => t.Id == destTag.Id, destTag);
        }
    }
}
