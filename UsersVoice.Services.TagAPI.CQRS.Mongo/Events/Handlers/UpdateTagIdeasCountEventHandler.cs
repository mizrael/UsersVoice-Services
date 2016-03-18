using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;

namespace UsersVoice.Services.TagAPI.CQRS.Mongo.Events.Handlers
{
    public class UpdateTagIdeasCountEventHandler : IAsyncNotificationHandler<CQRS.Events.IdeaTagCreated>,
                                                    IAsyncNotificationHandler<CQRS.Events.IdeaTagRemoved>
    {
        private readonly IQueriesDbContext _queryDb;
        private readonly ICommandsDbContext _commandsDb;

        public UpdateTagIdeasCountEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _queryDb = queryDb;
            _commandsDb = commandsDb;
        }

        public async Task Handle(CQRS.Events.IdeaTagCreated @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var tagId = @event.TagId;

            await UpdateCount(tagId);
        }

        public async Task Handle(CQRS.Events.IdeaTagRemoved @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var tagId = @event.TagId;

            await UpdateCount(tagId);
        }

        private async Task UpdateCount(Guid tagId)
        {
            var tag = await _queryDb.Tags.Find(it => it.Id == tagId).FirstOrDefaultAsync();

            tag.IdeasCount = await _commandsDb.IdeaTags.CountAsync(it => it.TagId == tag.Id);

            await _queryDb.Tags.UpsertOneAsync(i => i.Id == tag.Id, tag);
        }
    }
}