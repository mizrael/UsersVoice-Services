using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;

namespace UsersVoice.Services.API.CQRS.Mongo.Events.Handlers
{
    public class IdeaTagCreatedEventHandler : IAsyncNotificationHandler<CQRS.Events.IdeaTagCreated>
    {
        private readonly IQueriesDbContext _queryDb;
        private readonly ICommandsDbContext _commandsDb;

        public IdeaTagCreatedEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _queryDb = queryDb;
            _commandsDb = commandsDb;
        }

        public async Task Handle(CQRS.Events.IdeaTagCreated @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var idea = await _queryDb.Ideas.Find(d => d.Id == @event.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + @event.IdeaId);

            var ideaTagIds =
                await _commandsDb.IdeaTags.Find(t => t.IdeaId == idea.Id).Project(it => it.TagId).ToListAsync();

            var ideaTags = await _queryDb.Tags.Find(t => ideaTagIds.Contains(t.Id))
                .Project(t => new TagBase()
                {
                    Slug = t.Slug,
                    Text = t.Text
                }).ToListAsync();

            idea.Tags.Clear();
            ideaTags.ForEach(t => idea.Tags.Add(t));

            await _queryDb.Ideas.UpsertOneAsync(i => i.Id == idea.Id, idea);
        }
    }
}
