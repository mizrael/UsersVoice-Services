﻿using System;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class IdeaCommentsArchiveQuery : ArchiveQuery<IdeaCommentArchiveItem>
    {
        public IdeaCommentsArchiveQuery() : base(0, 10)
        {
        }

        public Guid IdeaId { get; set; }
        public Guid AuthorId { get; set; }
        public IdeaCommentsSortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
