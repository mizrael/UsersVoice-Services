﻿using System;

namespace UsersVoice.Services.API.CQRS.Queries.Models
{
    public class UserDetails
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int AvailablePoints { get; set; }
        public int IdeasCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsAdmin { get; set; }
        public string[] Tags { get; set; } 
    }
}
