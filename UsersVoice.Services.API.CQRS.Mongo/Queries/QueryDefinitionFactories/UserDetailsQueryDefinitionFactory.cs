using System;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.QueryDefinitionFactories
{
    public class UserDetailsQueryDefinitionFactory : IQueryDefinitionFactory<UserDetailsQuery, User>
    {
        private readonly IQueriesDbContext _db;

        public UserDetailsQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition<User> Build(UserDetailsQuery query)
        {
            if (null == query)
                throw new ArgumentNullException("query");
            if (query.UserId == Guid.Empty)
                throw new ArgumentException("please provide a user id");

            var builder = new FilterDefinitionBuilder<User>();

            var queryDef = new MongoQueryDefinition<User>(_db.Users, builder.Eq(a => a.Id, query.UserId));
            
            return queryDef;
        }
    }
}