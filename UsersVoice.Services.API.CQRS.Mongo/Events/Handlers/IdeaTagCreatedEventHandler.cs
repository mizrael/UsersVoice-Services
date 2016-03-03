using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Events.Handlers
{
    public class IdeaTagCreatedEventHandler : IAsyncNotificationHandler<CQRS.Events.IdeaTagCreated>
    {
        private readonly IQueriesDbContext _queryDb;

        public IdeaTagCreatedEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _queryDb = queryDb;
        }

        public async Task Handle(CQRS.Events.IdeaTagCreated @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var idea = await _queryDb.Ideas.Find(d => d.Id == @event.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + @event.IdeaId);
         
            if (idea.Tags.Any(t => t.Id == @event.TagId))
                return;

            var tag = await _queryDb.Tags.Find(d => d.Id == @event.TagId).FirstOrDefaultAsync();
            if (null == tag)
                throw new ArgumentException("invalid tag id: " + @event.TagId);

            idea.Tags.Add(tag);

            await _queryDb.Ideas.UpsertOneAsync(i => i.Id == idea.Id, idea);
        }
    }
}
