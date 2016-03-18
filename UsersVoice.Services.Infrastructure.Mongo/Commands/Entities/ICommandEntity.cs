using System;

namespace UsersVoice.Infrastructure.Mongo.Commands.Entities
{
    public interface ICommandEntity
    {
        Guid Id { get; }
    }
}
