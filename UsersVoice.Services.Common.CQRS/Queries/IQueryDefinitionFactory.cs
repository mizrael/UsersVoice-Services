namespace UsersVoice.Services.Common.CQRS.Queries
{
    public interface IQueryDefinitionFactory<in TQuery, TEntity>
        where TQuery : IQuery
    {
        IQueryDefinition<TEntity> Build(TQuery query);
    }
}