using System;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using Tag = UsersVoice.Infrastructure.Mongo.Commands.Entities.Tag;

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

            this.Tags = repoFactory.Create<Tag>(new RepositoryOptions(connectionString, connStr.DatabaseName, "tags"));
            var tagsIndexBuilder = new IndexKeysDefinitionBuilder<Tag>();
            this.Tags.CreateIndex(tagsIndexBuilder.Ascending(i => i.Slug));

            this.IdeaTags = repoFactory.Create<IdeaTag>(new RepositoryOptions(connectionString, connStr.DatabaseName, "ideaTags"));
            this.UserTags = repoFactory.Create<UserTag>(new RepositoryOptions(connectionString, connStr.DatabaseName, "userTags"));
        }

        public IRepository<User> Users { get; private set; }
        public IRepository<Idea> Ideas { get; private set; }
        public IRepository<IdeaComment> IdeaComments { get; private set; }
        public IRepository<Area> Areas { get; private set;  }

        public IRepository<Tag> Tags { get; private set; }
        public IRepository<IdeaTag> IdeaTags { get; private set; }
        public IRepository<UserTag> UserTags { get; private set; }
    }
}