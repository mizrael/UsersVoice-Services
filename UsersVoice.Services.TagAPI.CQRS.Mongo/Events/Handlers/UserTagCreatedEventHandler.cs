using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;

namespace UsersVoice.Services.TagAPI.CQRS.Mongo.Events.Handlers
{
    public class UserTagCreatedEventHandler : IAsyncNotificationHandler<CQRS.Events.UserTagCreated>
    {
        private readonly IQueriesDbContext _queryDb;
        private readonly ICommandsDbContext _commandsDb;

        public UserTagCreatedEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _queryDb = queryDb;
            _commandsDb = commandsDb;
        }

        public async Task Handle(CQRS.Events.UserTagCreated @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var user = await _queryDb.Users.Find(d => d.Id == @event.UserId).FirstOrDefaultAsync();
            if (null == user)
                throw new ArgumentException("invalid user id: " + @event.UserId);

            var userTagIds =
                await _commandsDb.UserTags.Find(t => t.UserId == user.Id).Project(it => it.TagId).ToListAsync();

            var ideaTags = await _queryDb.Tags.Find(t => userTagIds.Contains(t.Id))
                .Project(t => new TagBase()
                {
                    Slug = t.Slug,
                    Text = t.Text
                }).ToListAsync();

            user.Tags.Clear();
            ideaTags.ForEach(t => user.Tags.Add(t));

            await _queryDb.Users.UpsertOneAsync(i => i.Id == user.Id, user);
        }
    }
}
