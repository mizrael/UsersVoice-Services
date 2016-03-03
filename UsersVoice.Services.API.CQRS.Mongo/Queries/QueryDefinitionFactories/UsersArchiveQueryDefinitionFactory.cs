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
    public class UsersArchiveQueryDefinitionFactory : IQueryDefinitionFactory<UsersArchiveQuery, User>
    {
        private readonly IQueriesDbContext _db;

        public UsersArchiveQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition<User> Build(UsersArchiveQuery query)
        {
            if(null == query)
                throw new ArgumentNullException("query");

            var filters = new List<FilterDefinition<User>>();
            var builder = Builders<User>.Filter;

            if (!string.IsNullOrWhiteSpace(query.CompleteName))
            {
                var expr = new BsonRegularExpression(new Regex(query.CompleteName, RegexOptions.IgnoreCase));
                var nameFilter = Builders<User>.Filter.Regex(vl => vl.CompleteName, expr);
                filters.Add(nameFilter);
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
                filters.Add(builder.Eq(v => v.Email, query.Email));

            if (null != query.Tags && query.Tags.Any())
            {
                FilterDefinition<User> filter = null;

                if (query.TagOperation == TagFilterOperation.All)
                {
                    filter = Builders<User>.Filter.And(
                        query.Tags.Select(slug =>
                            Builders<User>.Filter.ElemMatch(i => i.Tags,
                                Builders<BaseTag>.Filter.Eq(t => t.Slug, slug))
                            ).ToArray()
                        );
                }
                else
                {
                    var innerFilter = Builders<BaseTag>.Filter.In(t => t.Slug, query.Tags);
                    filter = Builders<User>.Filter.ElemMatch(i => i.Tags, innerFilter);
                }

                filters.Add(filter);
            }

            var sorting = SortingBuilder.BuildSorting(query.SortBy, query.SortDirection, GetSortingField);

            var queryDef = new MongoPagingQueryDefinition<User>(_db.Users, builder.And(filters), query, sorting);
            return queryDef;
        }

        private static Expression<Func<User, object>> GetSortingField(UserSortBy sortBy)
        {
            switch (sortBy)
            {
                case UserSortBy.RegistrationDate:
                default:
                    return (User i) => i.RegistrationDate;
                case UserSortBy.Points:
                    return (User i) => i.AvailablePoints;
                case UserSortBy.Name:
                    return (User i) => i.LastName;
            }
        }
    }
}