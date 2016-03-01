using System;

namespace UsersVoice.Infrastructure.Mongo.Queries.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Slug { get; set; }
    }
}
