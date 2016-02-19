using System;
using MongoDB.Driver.Core.Configuration;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;

namespace UsersVoice.Infrastructure.Mongo.Commands
{
    public class CommandsDbContext : ICommandsDbContext
    {
        public CommandsDbContext(IRepositoryFactory repoFactory, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            var connStr = new ConnectionString(connectionString);

            this.Users = repoFactory.Create<User>(new RepositoryOptions(connectionString, connStr.DatabaseName, "users"));
            this.Ideas = repoFactory.Create<Idea>(new RepositoryOptions(connectionString, connStr.DatabaseName, "ideas"));
            this.IdeaComments = repoFactory.Create<IdeaComment>(new RepositoryOptions(connectionString, connStr.DatabaseName, "ideaComments"));
            this.Areas = repoFactory.Create<Area>(new RepositoryOptions(connectionString, connStr.DatabaseName, "areas"));
        }

        public IRepository<User> Users { get; private set; }
        public IRepository<Idea> Ideas { get; private set; }
        public IRepository<IdeaComment> IdeaComments { get; private set; }
        public IRepository<Area> Areas { get; private set;  }
    }
}