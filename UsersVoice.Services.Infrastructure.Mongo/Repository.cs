using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace UsersVoice.Infrastructure.Mongo
{
    public class Repository<TEntity> : IRepository<TEntity>
    {
        private readonly IMongoCollection<TEntity> _collection;

        public Repository(IMongoCollection<TEntity> collection)
        {
            if (null == collection)
                throw new ArgumentNullException("collection");
            _collection = collection;

            this.CollectionName = collection.CollectionNamespace.CollectionName;
        }

        public string CollectionName { get; private set; }

        public async Task CreateIndexAsync(IndexKeysDefinition<TEntity> indexDefinition, CreateIndexOptions options)
        {
            var db = _collection.Database;
            if (db == null)
                return;
            var coll = db.GetCollection<TEntity>(this.CollectionName);
            if (coll == null)
                return;
            await coll.Indexes.CreateOneAsync(indexDefinition, options);
        }

        public void CreateIndex(IndexKeysDefinition<TEntity> indexDefinition)
        {
            CreateIndex(indexDefinition, null);
        }

        public void CreateIndex(IndexKeysDefinition<TEntity> indexDefinition, CreateIndexOptions options)
        {
            var db = _collection.Database;
            if (db == null)
                return;
            var coll = db.GetCollection<TEntity>(this.CollectionName);
            if (coll == null)
                return;
            coll.Indexes.CreateOne(indexDefinition, options);
        }

        public Task<long> CountAsync(FilterDefinition<TEntity> filter)
        {
            return _collection.CountAsync(filter);
        }


        public IFindFluent<TEntity, TEntity> Find(Expression<Func<TEntity, bool>> filter)
        {
            return Find(filter, null);
        }
        public IFindFluent<TEntity, TEntity> Find(Expression<Func<TEntity, bool>> filter, FindOptions options)
        {
            return _collection.Find(filter, options);
        }

        public IFindFluent<TEntity, TEntity> Find(FilterDefinition<TEntity> filter)
        {
            return Find(filter, null);
        }

        public IFindFluent<TEntity, TEntity> Find(FilterDefinition<TEntity> filter, FindOptions options)
        {
            return _collection.Find(filter, options);
        }

        public Task<TEntity> FindOneAndReplaceAsync(FilterDefinition<TEntity> filter, TEntity replacement)
        {
            return _collection.FindOneAndReplaceAsync(filter, replacement);
        }
        public Task<TEntity> FindOneAndReplaceAsync(Expression<Func<TEntity, bool>> filter, TEntity replacement)
        {
            return _collection.FindOneAndReplaceAsync(filter, replacement);
        }

        public Task InsertOneAsync(TEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task<TEntity> UpsertOneAsync(Expression<Func<TEntity, bool>> filter, TEntity entity)
        {
            return _collection.FindOneAndReplaceAsync(filter, entity,
                                                      new FindOneAndReplaceOptions<TEntity, TEntity>() { IsUpsert = true });
        }

        public Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _collection.DeleteManyAsync(filter);
        }
        public Task DeleteOneAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _collection.DeleteOneAsync(filter);
        }
    }
}