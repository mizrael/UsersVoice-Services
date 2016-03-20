using System;

namespace UsersVoice.Infrastructure.Mongo.Queries.Entities
{
    public class Tag : TagBase, IQueryEntity
    {
        public Guid Id { get; set; }
        public long IdeasCount { get; set; }
        public long UsersCount { get; set; }
    }
}
