﻿using UsersVoice.Infrastructure.Mongo.Commands.Entities;

namespace UsersVoice.Infrastructure.Mongo.Commands
{
    public interface ICommandsDbContext
    {
        IRepository<User> Users { get; }
        IRepository<Idea> Ideas { get; }
        IRepository<IdeaComment> IdeaComments { get; }
        IRepository<Area> Areas { get; }
        IRepository<Tag> Tags { get; }
        IRepository<IdeaTag> IdeaTags { get; }
        IRepository<UserTag> UserTags { get; }
    }
}
