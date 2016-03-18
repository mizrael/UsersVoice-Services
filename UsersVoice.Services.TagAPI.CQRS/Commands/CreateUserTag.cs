using System;
using MediatR;

namespace UsersVoice.Services.TagAPI.CQRS.Commands
{
    public class CreateUserTag : IAsyncNotification
    {
        public CreateUserTag(Guid tagId, Guid userId)
        {
            this.TagId = tagId;
            this.UserId = userId;
        }

        public Guid TagId { get; private set; }
        public Guid UserId { get; private set; }
    }
}