using System;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.TagAPI.CQRS.Queries.Models
{
    public class TagArchiveItem : TagBase
    {
        public Guid Id { get; set; }
        public long IdeasCount { get; set; }
        public long UsersCount { get; set; }
    }
}
