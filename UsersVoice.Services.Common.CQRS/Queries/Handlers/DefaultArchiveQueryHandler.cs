using System;
using System.Threading.Tasks;
using MediatR;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.Common.CQRS.Queries.Handlers
{
    public class DefaultArchiveQueryHandler<TRequest, TEntity, TResponse> : IAsyncRequestHandler<TRequest, PagedCollection<TResponse>>
       where TRequest : MediatR.IAsyncRequest<PagedCollection<TResponse>>, IQuery
    {
        private readonly IQueryDefinitionFactory<TRequest, TEntity> _queryDefinitionFactory;
        private readonly IQueryRunner<PagedCollection<TResponse>> _queryRunner;

        public DefaultArchiveQueryHandler(IQueryDefinitionFactory<TRequest, TEntity> queryDefinitionFactory,
            IQueryRunner<PagedCollection<TResponse>> queryRunner)
        {
            if (queryDefinitionFactory == null)
                throw new ArgumentNullException("queryDefinitionFactory");
            if (queryRunner == null)
                throw new ArgumentNullException("queryRunner");

            _queryDefinitionFactory = queryDefinitionFactory;
            _queryRunner = queryRunner;
        }

        public async Task<PagedCollection<TResponse>> Handle(TRequest query)
        {
            if (query == null) throw new ArgumentNullException("query");
            var filter = _queryDefinitionFactory.Build(query);
            var result = await _queryRunner.RunAsync(filter);
            return result;
        }
    }
}