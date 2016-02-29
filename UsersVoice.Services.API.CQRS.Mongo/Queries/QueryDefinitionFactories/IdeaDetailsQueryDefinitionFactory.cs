using System;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.QueryDefinitionFactories
{
    public class IdeaDetailsQueryDefinitionFactory : IQueryDefinitionFactory<IdeaDetailsQuery>
    {
        private readonly IQueriesDbContext _db;

        public IdeaDetailsQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition Build(IdeaDetailsQuery query)
        {
            if (null == query)
                throw new ArgumentNullException("query");
            if(query.IdeaId == Guid.Empty)
                throw new ArgumentException("please provide a video id");

            var builder = new FilterDefinitionBuilder<Area>();

            var queryDef = new MongoQueryDefinition<Area>(_db.Areas, builder.Eq(a => a.Id, query.IdeaId));
            
            return queryDef;
        }
    }
}