using System;

namespace UsersVoice.Infrastructure.Mongo.Queries.Entities
{
    /// <summary>
    /// base class for Tags used as embedded document
    /// </summary>
    public class TagBase
    {
        public string Text { get; set; }
        public string Slug { get; set; }
    }

    public class Tag : TagBase
    {
        public Guid Id { get; set; }
    }
}
