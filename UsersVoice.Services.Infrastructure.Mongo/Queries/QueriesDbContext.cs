﻿using System;
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
            var userIndexBuilder = new IndexKeysDefinitionBuilder<User>();
            this.Users.CreateIndex(userIndexBuilder.Ascending(i => i.Tags));
            this.Users.CreateIndex(userIndexBuilder.Ascending(i => i.Email));

            this.Areas = repoFactory.Create<Area>(new RepositoryOptions(connectionString, connStr.DatabaseName, "areas"));

            this.Ideas = repoFactory.Create<Idea>(new RepositoryOptions(connectionString, connStr.DatabaseName, "ideas"));
            var ideaIndexBuilder = new IndexKeysDefinitionBuilder<Idea>();
            this.Ideas.CreateIndex(ideaIndexBuilder.Ascending(i => i.AuthorId));
            this.Ideas.CreateIndex(ideaIndexBuilder.Ascending(i => i.AreaId));
            this.Ideas.CreateIndex(ideaIndexBuilder.Ascending(i => i.Tags));
            this.Ideas.CreateIndex(ideaIndexBuilder.Ascending(i => i.CreationDate));

            this.IdeaComments = repoFactory.Create<IdeaComment>(new RepositoryOptions(connectionString, connStr.DatabaseName, "ideaComments"));
            var commentsIndexBuilder = new IndexKeysDefinitionBuilder<IdeaComment>();
            this.IdeaComments.CreateIndex(commentsIndexBuilder.Ascending(i => i.IdeaId));
            this.IdeaComments.CreateIndex(commentsIndexBuilder.Ascending(i => i.AuthorId));
            this.IdeaComments.CreateIndex(commentsIndexBuilder.Ascending(i => i.CreationDate));

            this.Tags = repoFactory.Create<Entities.Tag>(new RepositoryOptions(connectionString, connStr.DatabaseName, "tags"));
            var tagIndexBuilder = new IndexKeysDefinitionBuilder<Entities.Tag>();
            this.Tags.CreateIndex(tagIndexBuilder.Ascending(i => i.Slug));
        }

        public IRepository<User> Users { get; private set; }
        public IRepository<Idea> Ideas { get; private set; }
        public IRepository<IdeaComment> IdeaComments { get; private set; }
        public IRepository<Area> Areas { get; private set; }
        public IRepository<Entities.Tag> Tags { get; private set; }
    }
}