using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.TagAPI.CQRS.Events;
using UsersVoice.Services.TagAPI.CQRS.Mongo.Events.Handlers;

namespace UsersVoice.DataRefresher
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
            RegisterInfrastructure(container);

            container.Verify();

            Task.WaitAll(RefreshTagCounts(container));

            Console.ReadLine();
        }

        private static async Task RefreshTagCounts(Container container)
        {
            Console.WriteLine("refreshing tag counts...");
          
            var commandDbContext = container.GetInstance<ICommandsDbContext>();
            var queryDbContext = container.GetInstance<IQueriesDbContext>();

            var tags = await queryDbContext.Tags.Find("{}").ToListAsync();

            var ideaHandler = new UpdateTagIdeasCountEventHandler(commandDbContext, queryDbContext);
            var userHandler = new UpdateTagUsersCountEventHandler(commandDbContext, queryDbContext);

            foreach (var tag in tags)
            {
                await Task.WhenAll(
                      ideaHandler.Handle(new IdeaTagCreated(Guid.NewGuid(), tag.Id)),
                      userHandler.Handle(new UserTagCreated(Guid.NewGuid(), tag.Id))
                );
            }

            Console.WriteLine("tag count refresh done!");
        }

        private static void RegisterInfrastructure(Container container)
        {
            container.RegisterSingleton<IMongoDatabaseFactory, MongoDatabaseFactory>();
            container.RegisterSingleton<IRepositoryFactory, RepositoryFactory>();

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
        }
    }
}
