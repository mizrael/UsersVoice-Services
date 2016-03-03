using System;
using System.Collections.Generic;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class IdeasArchiveQuery : ArchiveQuery<IdeaArchiveItem>
    {
        public IdeasArchiveQuery() : base(0, 10)
        {
        }

        public string Title { get; set; }

        public Guid AuthorId { get; set; }

        public Guid AreaId { get; set; }

        public IEnumerable<string> Tags { get; set; }
        public TagFilterOperation TagOperation { get; set; }

        public IdeaStatusQuery Status { get; set; }

        public IdeaSortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }

    public enum TagFilterOperation
    {
        Any,
        All
    }
}
