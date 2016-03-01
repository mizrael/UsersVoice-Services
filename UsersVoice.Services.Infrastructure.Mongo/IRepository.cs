using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace UsersVoice.Infrastructure.Mongo
{
    public interface IRepository<TEntity>
    {
        string CollectionName { get; }

        void CreateIndex(IndexKeysDefinition<TEntity> indexDefinition);
        void CreateIndex(IndexKeysDefinition<TEntity> indexDefinition, CreateIndexOptions options);
        Task CreateIndexAsync(IndexKeysDefinition<TEntity> indexDefinition, CreateIndexOptions options);

        Task<long> CountAsync(FilterDefinition<TEntity> filter);

        IFindFluent<TEntity, TEntity> Find(Expression<Func<TEntity, bool>> filter);
        IFindFluent<TEntity, TEntity> Find(Expression<Func<TEntity, bool>> filter, FindOptions options);

        IFindFluent<TEntity, TEntity> Find(FilterDefinition<TEntity> filter);
        IFindFluent<TEntity, TEntity> Find(FilterDefinition<TEntity> filter, FindOptions options);

        Task<TEntity> FindOneAndReplaceAsync(FilterDefinition<TEntity> filter, TEntity replacement);
        Task<TEntity> FindOneAndReplaceAsync(Expression<Func<TEntity, bool>> filter, TEntity replacement);

        Task InsertOneAsync(TEntity entity);

        Task<TEntity> UpsertOneAsync(Expression<Func<TEntity, bool>> filter, TEntity entity);

        Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter);

        Task DeleteOneAsync(Expression<Func<TEntity, bool>> filter);
    }
}