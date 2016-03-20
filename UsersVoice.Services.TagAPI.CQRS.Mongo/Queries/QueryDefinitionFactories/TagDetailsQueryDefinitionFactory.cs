using System;
using System.Collections.Generic;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.Common.CQRS.Queries;
using UsersVoice.Services.TagAPI.CQRS.Queries;
using Tag = UsersVoice.Infrastructure.Mongo.Queries.Entities.Tag;

namespace UsersVoice.Services.TagAPI.CQRS.Mongo.Queries.QueryDefinitionFactories
{
    public class TagDetailsQueryDefinitionFactory : IQueryDefinitionFactory<TagDetailsQuery, Tag>
    {
        private readonly IQueriesDbContext _db;

        public TagDetailsQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition<Tag> Build(TagDetailsQuery query)
        {
            if (null == query)
                throw new ArgumentNullException("query");

            var filters = new List<FilterDefinition<Tag>>();
            var builder = Builders<Tag>.Filter;

            if (query.TagId != Guid.Empty)
            {
                var idFilter = Builders<Tag>.Filter.Eq(i => i.Id, query.TagId);
                filters.Add(idFilter);
            }

            if (!string.IsNullOrWhiteSpace(query.Slug))
            {
                var filter = Builders<Tag>.Filter.Eq(i => i.Slug, query.Slug);
                filters.Add(filter);
            }

            var finalFilter = new MongoQueryDefinition<Tag>(_db.Tags, builder.And(filters));
            return finalFilter;
        }
    }
}