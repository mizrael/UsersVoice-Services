using System;

namespace UsersVoice.Infrastructure.Mongo.Commands.Entities
{
    public class Tag : ICommandEntity
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public string Slug { get; set; }
    }

    public class IdeaTag : ICommandEntity
    {
        public Guid Id { get; set; }
        public Guid IdeaId { get; set; }
        public Guid TagId { get; set; }
    }

    public class UserTag : ICommandEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TagId { get; set; }
    }
}
