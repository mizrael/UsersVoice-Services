using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.QueryDefinitionFactories
{
    public class IdeaCommentsArchiveQueryDefinitionFactory : IQueryDefinitionFactory<IdeaCommentsArchiveQuery>
    {
        private readonly IQueriesDbContext _db;

        public IdeaCommentsArchiveQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition Build(IdeaCommentsArchiveQuery query)
        {
            if (null == query)
                throw new ArgumentNullException("query");

            var filters = new List<FilterDefinition<IdeaComment>>();
            var builder = Builders<IdeaComment>.Filter;

            if (query.IdeaId != Guid.Empty)
            {
                var ideaIdFilter = Builders<IdeaComment>.Filter.Eq(i => i.IdeaId, query.IdeaId);
                filters.Add(ideaIdFilter);
            }

            if (query.AuthorId != Guid.Empty)
            {
                var authorIdFilter = Builders<IdeaComment>.Filter.Eq(i => i.AuthorId, query.AuthorId);
                filters.Add(authorIdFilter);
            }

            var sorting = SortingBuilder.BuildSorting(query.SortBy, query.SortDirection, GetSortingField);

            var filter = new MongoPagingQueryDefinition<IdeaComment>(_db.IdeaComments, builder.And(filters), query, sorting);
            return filter;
        }

        private static Expression<Func<IdeaComment, object>> GetSortingField(IdeaCommentsSortBy sortBy)
        {
            switch (sortBy)
            {
                case IdeaCommentsSortBy.CreationDate:
                default:
                    return (IdeaComment i) => i.CreationDate;
            }
        }
    }
}