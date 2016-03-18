using System;
using MediatR;

namespace UsersVoice.Services.TagAPI.CQRS.Events
{
    public class UserTagRemoved : IAsyncNotification
    {
        public UserTagRemoved(Guid userId, Guid tagId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException("userId");
            if (tagId == Guid.Empty) throw new ArgumentNullException("tagId");
            UserId = userId;
            TagId = tagId;
        }

        public Guid UserId { get; private set; }
        public Guid TagId { get; private set; }
    }
}