using System;

namespace UsersVoice.Infrastructure.Mongo.Commands.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }

    public class IdeaTag
    {
        public Guid Id { get; set; }
        public Guid IdeaId { get; set; }
        public Guid TagId { get; set; }
    }

    public class UserTag
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TagId { get; set; }
    }
}
