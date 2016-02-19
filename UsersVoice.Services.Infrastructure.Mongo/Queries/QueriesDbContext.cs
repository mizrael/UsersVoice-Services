using System;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;

namespace UsersVoice.Infrastructure.Mongo.Queries
{
    public class QueriesDbContext : IQueriesDbContext
    {
        public QueriesDbContext(IRepositoryFactory repoFactory, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            var connStr = new ConnectionString(connectionString);

            this.Users = repoFactory.Create<User>(new RepositoryOptions(connectionString, connStr.DatabaseName, "users"));
            this.Areas = repoFactory.Create<Area>(new RepositoryOptions(connectionString, connStr.DatabaseName, "areas"));

            this.Ideas = repoFactory.Create<Idea>(new RepositoryOptions(connectionString, connStr.DatabaseName, "ideas"));
            var ideaIndexBuilder = new IndexKeysDefinitionBuilder<Idea>();
            this.Ideas.CreateIndex(ideaIndexBuilder.Ascending(i => i.AuthorId));
            this.Ideas.CreateIndex(ideaIndexBuilder.Ascending(i => i.AreaId));
            this.Ideas.CreateIndex(ideaIndexBuilder.Ascending(i => i.CreationDate));

            this.IdeaComments = repoFactory.Create<IdeaComment>(new RepositoryOptions(connectionString, connStr.DatabaseName, "ideaComments"));
            var commentsIndexBuilder = new IndexKeysDefinitionBuilder<IdeaComment>();
            this.IdeaComments.CreateIndex(commentsIndexBuilder.Ascending(i => i.IdeaId));
            this.IdeaComments.CreateIndex(commentsIndexBuilder.Ascending(i => i.AuthorId));
            this.IdeaComments.CreateIndex(commentsIndexBuilder.Ascending(i => i.CreationDate));
        }

        public IRepository<User> Users { get; private set; }
        public IRepository<Idea> Ideas { get; private set; }
        public IRepository<IdeaComment> IdeaComments { get; private set; }
        public IRepository<Area> Areas { get; private set; }
    }
}