using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Infrastructure.Mongo
{
    public class MongoQueryExecutor : IMongoQueryExecutor
    {
        public async Task<TEntity> ReadOneAsync<TEntity>(IMongoQueryDefinition<TEntity> queryDefinition)
        {
            ValidateQueryDefinition(queryDefinition);

            var entity = await queryDefinition.Repository.Find(queryDefinition.Filter).FirstOrDefaultAsync();
            return entity;
        }

        public async Task<PagedCollection<TEntity>> ReadAsync<TEntity>(IMongoQueryDefinition<TEntity> queryDefinition)
        {
            ValidateQueryDefinition(queryDefinition);

            long totalPagesCount = 0, totalItemsCount = 0, page = 0, pageSize = 0;

            var cursor = queryDefinition.Repository.Find(queryDefinition.Filter);

            if (null != queryDefinition.Sorting)
                cursor = cursor.Sort(queryDefinition.Sorting);
        
            var pagingInfo = queryDefinition as IPagingInfo;
            var isPaging = (null != pagingInfo);
            if (isPaging)
            {
                cursor = cursor.Skip(pagingInfo.PageSize*pagingInfo.Page)
                               .Limit(pagingInfo.PageSize);

                page = pagingInfo.Page;
                pageSize = pagingInfo.PageSize;

                totalItemsCount = await queryDefinition.Repository.CountAsync(queryDefinition.Filter);
                if (0 == totalItemsCount)
                    return PagedCollection<TEntity>.Empty;

                totalPagesCount = (long) Math.Ceiling((float) totalItemsCount / pagingInfo.PageSize);
            }

            var entities = await cursor.ToListAsync();
            if (null == entities)
                return PagedCollection<TEntity>.Empty;

            if (!isPaging)
                totalPagesCount = entities.Count;

            return new PagedCollection<TEntity>(entities, page, pageSize, totalPagesCount, totalItemsCount);
        }

        private static void ValidateQueryDefinition<TEntity>(IMongoQueryDefinition<TEntity> queryDefinition)
        {
            if (queryDefinition == null)
                throw new ArgumentNullException("queryDefinition");

            if (queryDefinition.Repository == null)
                throw new ArgumentException("invalid repository");

            if (queryDefinition.Filter == null)
                throw new ArgumentException("invalid filter");
        }

    }
}