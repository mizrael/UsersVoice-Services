using System;
using System.Linq.Expressions;
using MongoDB.Driver;
using SortDirection = UsersVoice.Services.Common.CQRS.Queries.SortDirection;

namespace UsersVoice.Infrastructure.Mongo.Queries
{
    public static class SortingBuilder
    {

        public static SortDefinition<TEntity> BuildSorting<TEntity, TSortBy>(TSortBy sortBy, SortDirection direction,
                                                                             Func<TSortBy, Expression<Func<TEntity, object>>> getSortingField)
        {
            var sortingField = getSortingField(sortBy);
            if (null == sortingField)
                return null;

            var sortingBuilder = Builders<TEntity>.Sort;

            return direction == SortDirection.ASC
                ? sortingBuilder.Ascending(sortingField)
                : sortingBuilder.Descending(sortingField);
        }
    }
}
