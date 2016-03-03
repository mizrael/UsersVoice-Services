using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.Common.CQRS.Queries;
using Tag = UsersVoice.Infrastructure.Mongo.Queries.Entities.Tag;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.QueryDefinitionFactories
{
    public class IdeasArchiveQueryDefinitionFactory : IQueryDefinitionFactory<IdeasArchiveQuery, Idea>
    {
        private readonly IQueriesDbContext _db;

        public IdeasArchiveQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition<Idea> Build(IdeasArchiveQuery query)
        {
            if (null == query)
                throw new ArgumentNullException("query");

            var filters = new List<FilterDefinition<Idea>>();
            var filterBuilder = Builders<Idea>.Filter;

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                var expr = new BsonRegularExpression(new Regex(query.Title, RegexOptions.IgnoreCase));
                var titleFilter = Builders<Idea>.Filter.Regex(vl => vl.Title, expr);
                filters.Add(titleFilter);
            }

            if (query.Status != IdeaStatusQuery.All)
            {
                var status = (Idea.IdeaStatus) query.Status;
                var statusFilter = Builders<Idea>.Filter.Eq(i => i.Status, status);
                filters.Add(statusFilter);
            }

            if (query.AuthorId != Guid.Empty)
            {
                var authorIdFilter = Builders<Idea>.Filter.Eq(i => i.AuthorId, query.AuthorId);
                filters.Add(authorIdFilter);
            }

            if (query.AreaId != Guid.Empty)
            {
                var areaIdFilter = Builders<Idea>.Filter.Eq(i => i.AreaId, query.AreaId);
                filters.Add(areaIdFilter);
            }

            if (null != query.Tags && query.Tags.Any())
            {
                FilterDefinition<Idea> filter = null;

                if (query.TagOperation == TagFilterOperation.All)
                {
                    filter = Builders<Idea>.Filter.And(
                        query.Tags.Select(slug =>
                            Builders<Idea>.Filter.ElemMatch(i => i.Tags,
                                Builders<BaseTag>.Filter.Eq(t => t.Slug, slug))
                            ).ToArray()
                        );
                }
                else
                {
                    var innerFilter = Builders<BaseTag>.Filter.In(t => t.Slug, query.Tags);
                    filter = Builders<Idea>.Filter.ElemMatch(i => i.Tags, innerFilter);
                }

                filters.Add(filter);
            }

            var sorting = SortingBuilder.BuildSorting(query.SortBy, query.SortDirection, GetSortingField);

            var queryDef = new MongoPagingQueryDefinition<Idea>(_db.Ideas, filterBuilder.And(filters), query, sorting);
            return queryDef;
        }

        private static Expression<Func<Idea, object>> GetSortingField(IdeaSortBy sortBy)
        {
            switch (sortBy)
            {
                case IdeaSortBy.Points:
                    return (Idea i) => i.TotalPoints;
                case IdeaSortBy.Title:
                    return (Idea i) => i.Title;
                default:
                    return (Idea i) => i.CreationDate;
            }
        }
    }
}