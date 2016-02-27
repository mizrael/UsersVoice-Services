using System;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class TagsArchiveQuery : ArchiveQuery<TagArchiveItem>
    {
        public TagsArchiveQuery() : base(0, 10)
        {
        }

        public string Text { get; set; }

        public Guid TagId { get; set; }

        public TagSortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
