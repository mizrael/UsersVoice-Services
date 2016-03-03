using System;
using System.Collections.Generic;

namespace UsersVoice.Infrastructure.Mongo.Queries.Entities
{
    public class User
    {
        public User()
        {
            Tags = new List<BaseTag>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompleteName
        {
            get { return string.Format("{0} {1}", this.FirstName, this.LastName); }
        }

        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int AvailablePoints { get; set; }
        public int IdeasCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsAdmin { get; set; }
        public IList<BaseTag> Tags { get; private set; }
    }
}
