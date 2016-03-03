using System.Collections.Generic;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class UsersArchiveQuery : ArchiveQuery<UserArchiveItem>
    {
        public UsersArchiveQuery() : base(0, 10) { }

        public string CompleteName { get; set; }
        
        public string Email{ get; set; }

        public IEnumerable<string> Tags { get; set; } 

        public UserSortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}