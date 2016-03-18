using System;

namespace UsersVoice.Infrastructure.Mongo.Commands.Entities
{
    public class User : ICommandEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int AvailablePoints { get; set; }
        public bool IsAdmin { get; set; }
    }
}
