using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using MediatR;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Infrastructure.Mongo.Services;
using UsersVoice.Services.API.CQRS.Mongo.Queries.Runners;
using UsersVoice.Services.Common.CQRS.Queries;
using UsersVoice.Services.Common.CQRS.Queries.Handlers;
using UsersVoice.Services.Infrastructure.Common;
using UsersVoice.Services.Infrastructure.Common.Services;
using UsersVoice.Services.TagAPI.CQRS.Events;
using UsersVoice.Services.TagAPI.CQRS.Mongo.Queries.QueryDefinitionFactories;
using UsersVoice.Services.TagAPI.CQRS.Queries;
using UsersVoice.Services.TagAPI.CQRS.Queries.Models;

namespace UsersVoice.Services.TagAPI
{
    public class DependenciesConfig
    {
        public static void Register(HttpConfiguration config)
        {
            RegisterDependencies(config);
        }

        private static void RegisterDependencies(HttpConfiguration config)
        {
            var container = new Container();
            container.RegisterWebApiControllers(config);

            RegisterInfrastructure(container);

            RegisterHandlers(container);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void RegisterInfrastructure(Container container)
        {
            container.RegisterSingleton<ISlugGenerator>(() => new SlugGenerator(45));
            container.Register<ITagSlugFinder, TagSlugFinder>();

            container.RegisterSingleton<IMongoDatabaseFactory, MongoDatabaseFactory>();
            container.RegisterSingleton<IRepositoryFactory, RepositoryFactory>();
            container.RegisterSingleton<IMongoQueryExecutor, MongoQueryExecutor>();

            container.Register<ICommandsDbContext>(() =>
            {
                var repoFactory = container.GetInstance<IRepositoryFactory>();
                var connString = ConfigurationManager.ConnectionStrings["Commands"].ConnectionString;
                return new CommandsDbContext(repoFactory, connString);
            });

            container.Register<IQueriesDbContext>(() =>
            {
                var repoFactory = container.GetInstance<IRepositoryFactory>();
                var connString = ConfigurationManager.ConnectionStrings["Queries"].ConnectionString;
                return new QueriesDbContext(repoFactory, connString);
            });

            RegisterMediator(container);
        }

        private static void RegisterMediator(Container container)
        {
            container.RegisterSingleton<IMediator, Mediator>();

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        #region Handlers

        private static void RegisterHandlers(Container container)
        {
            var assemblies = GetAssemblies().ToArray();
            container.Register(typeof(IAsyncRequestHandler<,>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.Register(typeof(IValidator<>), assemblies);
            container.RegisterConditional(typeof(IValidator<>), typeof(NullValidator<>), c => !c.Handled);

            RegisterTagHandlers(container);
        }

        private static void RegisterTagHandlers(Container container)
        {
            RegisterArchiveHandlers<Tag, TagsArchiveQueryDefinitionFactory, TagsArchiveQuery, TagArchiveItem>(container);
        }

        private static void RegisterArchiveHandlers<TEntity, TQueryDefFactory, TQuery, TResult>(Container container)
            where TEntity : IQueryEntity
            where TQueryDefFactory : class, IQueryDefinitionFactory<TQuery, TEntity>
            where TQuery : MediatR.IAsyncRequest<PagedCollection<TResult>>, IQuery
        {
            container.Register<IQueryDefinitionFactory<TQuery, TEntity>, TQueryDefFactory>();
            container.Register<IQueryRunner<PagedCollection<TResult>>, DefaultArchiveQueryRunner<TResult>>();
            container.Register<IAsyncRequestHandler<TQuery, PagedCollection<TResult>>,
                DefaultArchiveQueryHandler<TQuery, TEntity, TResult>>();
        }

        private static void RegisterDetailsHandlers<TEntity, TQueryDefFactory, TQuery, TResult>(Container container)
            where TQueryDefFactory : class, IQueryDefinitionFactory<TQuery, TEntity>
            where TQuery : MediatR.IAsyncRequest<TResult>, IQuery
        {
            container.Register<IQueryDefinitionFactory<TQuery, TEntity>, TQueryDefFactory>();
            container.Register<IQueryRunner<TResult>, DefaultDetailsQueryRunner<TResult>>();
            container.Register<IAsyncRequestHandler<TQuery, TResult>,
                DefaultDetailsQueryHandler<TQuery, TEntity, TResult>>();
        }

        #endregion Handlers

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).GetTypeInfo().Assembly;
            yield return typeof(DefaultArchiveQueryHandler<,,>).GetTypeInfo().Assembly;
            yield return typeof(IdeaTagCreated).GetTypeInfo().Assembly;
        }
    }
}