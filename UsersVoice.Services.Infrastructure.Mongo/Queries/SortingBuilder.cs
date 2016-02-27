using System;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace UsersVoice.Infrastructure.Mongo.Queries
{
    public static class SortingBuilder
    {

        public static SortDefinition<TEntity> BuildSorting<TEntity, TSortBy>(TSortBy sortBy, SortDirection direction,
                                                                             Func<TSortBy, Expression<Func<TEntity, object>>> getSortingField)
        {
            var sortingBuilder = Builders<TEntity>.Sort;

            var sortingField = getSortingField(sortBy);

            return direction == SortDirection.ASC
                ? sortingBuilder.Ascending(sortingField)
                : sortingBuilder.Descending(sortingField);
        }
    }
    
    public enum SortDirection
    {
        ASC,
        DESC
    }
}
