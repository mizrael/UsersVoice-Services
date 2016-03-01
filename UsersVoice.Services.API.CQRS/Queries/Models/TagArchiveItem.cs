using System;

namespace UsersVoice.Services.API.CQRS.Queries.Models
{
    public class TagArchiveItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Slug { get; set; }
    }
}
