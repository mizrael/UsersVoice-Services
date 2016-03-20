using System;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.Common.CQRS.Queries;
using UsersVoice.Services.TagAPI.CQRS.Queries.Models;

namespace UsersVoice.Services.TagAPI.CQRS.Queries
{
    public class TagsArchiveQuery : ArchiveQuery<TagArchiveItem>
    {
        public TagsArchiveQuery() : base(0, 10)
        {
        }

        public string Text { get; set; }
        public TagSortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
    
    public enum TagSortBy
    {
        None,
        Text,
        IdeasCount,
        UsersCount
    }
}
