using System;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Queries.Models
{
    public class UserArchiveItem
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int AvailablePoints { get; set; }
        public bool IsAdmin { get; set; }
        public TagBase[] Tags { get; set; } 
    }
}
