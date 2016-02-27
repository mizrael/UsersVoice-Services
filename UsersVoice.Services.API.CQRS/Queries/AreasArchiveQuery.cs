using System;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class AreasArchiveQuery : ArchiveQuery<AreaArchiveItem>
    {
        public AreasArchiveQuery() : base(0, 10)
        {
        }

        public string Title { get; set; }

        public Guid AuthorId { get; set; }
    }
}
