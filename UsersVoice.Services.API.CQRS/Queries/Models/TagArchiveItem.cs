using System;

namespace UsersVoice.Services.API.CQRS.Queries.Models
{
    public class TagBase
    {
        public string Text { get; set; }
        public string Slug { get; set; }
    }

    public class TagArchiveItem : TagBase
    {
        public Guid Id { get; set; }
    }
}
