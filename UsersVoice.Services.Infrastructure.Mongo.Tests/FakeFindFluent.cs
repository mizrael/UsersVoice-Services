using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace UsersVoice.Services.Infrastructure.Mongo.Tests
{
    public class FakeFindFluent<TEntity, TProjection> : IFindFluent<TEntity, TProjection>
    {
        protected readonly IEnumerable<TProjection> _items;

        public FakeFindFluent(IEnumerable<TProjection> items)
        {
            _items = items ?? Enumerable.Empty<TProjection>();
        }

        public long Count(CancellationToken cancellationToken)
        {
            return (long)_items.Count();
        }

        public Task<long> CountAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Count(cancellationToken));
        }

        public virtual IFindFluent<TEntity, TNewProjection> Project<TNewProjection>(
            ProjectionDefinition<TEntity, TNewProjection> projection)
        {
            throw new NotImplementedException();
        }

        public IFindFluent<TEntity, TProjection> Limit(int? limit)
        {
            if (!limit.HasValue)
                return this;

            var newItems = _items.Take(limit.Value);
            return new FakeFindFluent<TEntity, TProjection>(newItems);
        }

        public FindOptions<TEntity, TProjection> Options
        {
            get { throw new NotImplementedException(); }
        }

        public IFindFluent<TEntity, TProjection> Skip(int? skip)
        {
            if (!skip.HasValue) return this;
            return new FakeFindFluent<TEntity, TProjection>(_items.Skip(skip.Value));
        }

        public IFindFluent<TEntity, TProjection> Sort(SortDefinition<TEntity> sort)
        {
            throw new NotImplementedException();
        }

        public IAsyncCursor<TProjection> ToCursor(CancellationToken cancellationToken)
        {
            IAsyncCursor<TProjection> cursor = new FakeAsyncCursor<TProjection>(_items);
            return cursor;
        }

        public Task<IAsyncCursor<TProjection>> ToCursorAsync(CancellationToken cancellationToken)
        {
            var cursor = this.ToCursor(cancellationToken);
            var task = Task.FromResult(cursor);
            return task;
        }

        public IFindFluent<TEntity, TResult> As<TResult>(MongoDB.Bson.Serialization.IBsonSerializer<TResult> resultSerializer = null)
        {
            throw new NotImplementedException();
        }

        public FilterDefinition<TEntity> Filter
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

    }

    public class FakeFindFluent<TEntity> : FakeFindFluent<TEntity, TEntity>
    {
        public FakeFindFluent(IEnumerable<TEntity> items)
            : base(items)
        {
        }

        public Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_items.FirstOrDefault());
        }

        public override IFindFluent<TEntity, TNewProjection> Project<TNewProjection>(
         ProjectionDefinition<TEntity, TNewProjection> projection)
        {
            var findProj = projection as FindExpressionProjectionDefinition<TEntity, TNewProjection>;
            if (null == findProj)
                return null;

            var predicate = findProj.Expression.Compile();
            var projItems = _items.Select(predicate).ToArray();

            return new FakeFindFluent<TEntity, TNewProjection>(projItems);
        }
    }
}