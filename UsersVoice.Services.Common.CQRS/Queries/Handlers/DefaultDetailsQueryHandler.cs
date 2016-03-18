using System;
using System.Threading.Tasks;
using MediatR;

namespace UsersVoice.Services.Common.CQRS.Queries.Handlers
{
    public class DefaultDetailsQueryHandler<TRequest, TEntity, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : MediatR.IAsyncRequest<TResponse>, IQuery
    {
        private readonly IQueryDefinitionFactory<TRequest, TEntity> _queryDefinitionFactory;
        private readonly IQueryRunner<TResponse> _queryRunner;

        public DefaultDetailsQueryHandler(IQueryDefinitionFactory<TRequest, TEntity> queryDefinitionFactory,
            IQueryRunner<TResponse> queryRunner)
        {
            if (queryDefinitionFactory == null)
                throw new ArgumentNullException("queryDefinitionFactory");
            if (queryRunner == null)
                throw new ArgumentNullException("queryRunner");

            _queryDefinitionFactory = queryDefinitionFactory;
            _queryRunner = queryRunner;
        }

        public async Task<TResponse> Handle(TRequest query)
        {
            if (query == null) throw new ArgumentNullException("query");
            var filter = _queryDefinitionFactory.Build(query);
            var result = await _queryRunner.RunAsync(filter);
            return result;
        }
    }
}