using System;

namespace UsersVoice.Infrastructure.Mongo.Commands.Entities
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
