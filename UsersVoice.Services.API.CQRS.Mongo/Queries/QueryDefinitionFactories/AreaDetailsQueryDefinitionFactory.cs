using System;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.QueryDefinitionFactories
{
    public class AreaDetailsQueryDefinitionFactory : IQueryDefinitionFactory<AreaDetailsQuery>
    {
        private readonly IQueriesDbContext _db;

        public AreaDetailsQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition Build(AreaDetailsQuery query)
        {
            if (null == query)
                throw new ArgumentNullException("query");
            if(query.AreaId == Guid.Empty)
                throw new ArgumentException("please provide a video id");
            
            var builder = new FilterDefinitionBuilder<Area>();

            var queryDef = new MongoQueryDefinition<Area>(_db.Areas, builder.Eq(a => a.Id, query.AreaId));
            return queryDef;
        }
    }
}