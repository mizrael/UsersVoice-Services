using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;

namespace UsersVoice.Services.TagAPI.CQRS.Mongo.Events.Handlers
{
    public class UpdateTagUsersCountEventHandler : IAsyncNotificationHandler<CQRS.Events.UserTagCreated>, IAsyncNotificationHandler<CQRS.Events.UserTagRemoved>
    {
        private readonly IQueriesDbContext _queryDb;
        private readonly ICommandsDbContext _commandsDb;

        public UpdateTagUsersCountEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _queryDb = queryDb;
            _commandsDb = commandsDb;
        }

        public async Task Handle(CQRS.Events.UserTagCreated @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var tagId = @event.TagId;

            await UpdateCount(tagId);
        }
        public async Task Handle(CQRS.Events.UserTagRemoved @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var tagId = @event.TagId;

            await UpdateCount(tagId);
        }

        private async Task UpdateCount(Guid tagId)
        {
            var tag = await _queryDb.Tags.Find(it => it.Id == tagId).FirstOrDefaultAsync();

            tag.UsersCount = await _commandsDb.UserTags.CountAsync(it => it.TagId == tag.Id);

            await _queryDb.Tags.UpsertOneAsync(i => i.Id == tag.Id, tag);
        }
    }
}