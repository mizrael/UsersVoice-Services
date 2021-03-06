using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.Common.CQRS.Queries;
using UsersVoice.Services.TagAPI.CQRS.Queries;
using Tag = UsersVoice.Infrastructure.Mongo.Queries.Entities.Tag;

namespace UsersVoice.Services.TagAPI.CQRS.Mongo.Queries.QueryDefinitionFactories
{
    public class TagsArchiveQueryDefinitionFactory : IQueryDefinitionFactory<TagsArchiveQuery, Tag>
    {
        private readonly IQueriesDbContext _db;

        public TagsArchiveQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition<Tag> Build(TagsArchiveQuery query)
        {
            if (null == query)
                throw new ArgumentNullException("query");

            var filters = new List<FilterDefinition<Tag>>();
            var builder = Builders<Tag>.Filter;

            if (!string.IsNullOrWhiteSpace(query.Text))
            {
                var expr = new BsonRegularExpression(new Regex(query.Text, RegexOptions.IgnoreCase));
                var textFilter = Builders<Tag>.Filter.Regex(vl => vl.Text, expr);
                filters.Add(textFilter);
            }

            var sorting = SortingBuilder.BuildSorting(query.SortBy, query.SortDirection, GetSortingField);

            var filter = new MongoPagingQueryDefinition<Tag>(_db.Tags, builder.And(filters), query, sorting);
            return filter;
        }

        private static Expression<Func<Tag, object>> GetSortingField(TagSortBy sortBy)
        {
            switch (sortBy)
            {
                case TagSortBy.None:
                    return null;
                case TagSortBy.Text:
                    return (Tag i) => i.Text;
                case TagSortBy.IdeasCount:
                    return (Tag i) => i.IdeasCount;
                case TagSortBy.UsersCount:
                    return (Tag i) => i.UsersCount;
            }
            return null;
        }
    }
}