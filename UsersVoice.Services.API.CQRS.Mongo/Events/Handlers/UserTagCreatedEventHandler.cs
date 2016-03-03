using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Events.Handlers
{
    public class UserTagCreatedEventHandler : IAsyncNotificationHandler<CQRS.Events.UserTagCreated>
    {
        private readonly IQueriesDbContext _queryDb;

        public UserTagCreatedEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _queryDb = queryDb;
        }

        public async Task Handle(CQRS.Events.UserTagCreated @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var user = await _queryDb.Users.Find(d => d.Id == @event.UserId).FirstOrDefaultAsync();
            if (null == user)
                throw new ArgumentException("invalid user id: " + @event.UserId);

            if (user.Tags.Any(t => t.Id == @event.TagId))
                return;

            var tag = await _queryDb.Tags.Find(d => d.Id == @event.TagId).FirstOrDefaultAsync();
            if (null == tag)
                throw new ArgumentException("invalid tag id: " + @event.TagId);

            user.Tags.Add(tag);

            await _queryDb.Users.UpsertOneAsync(i => i.Id == user.Id, user);
        }
    }
}
