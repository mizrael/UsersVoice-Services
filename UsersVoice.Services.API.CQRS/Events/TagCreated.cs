using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Events
{
    public class TagCreated : IAsyncNotification
    {
        public TagCreated(Guid tagId)
        {
            if (tagId == Guid.Empty) throw new ArgumentNullException("tagId");
            TagId = tagId;
        }
        public Guid TagId { get; private set; }
    }

    public class IdeaTagCreated : IAsyncNotification
    {
        public IdeaTagCreated(Guid ideaId, Guid tagId)
        {
            if (ideaId == Guid.Empty) throw new ArgumentNullException("ideaId");
            if (tagId == Guid.Empty) throw new ArgumentNullException("tagId");

            IdeaId = ideaId;
            TagId = tagId;
        }

        public Guid IdeaId { get; private set; }
        public Guid TagId { get; private set; }
    }

    public class UserTagCreated : IAsyncNotification
    {
        public UserTagCreated(Guid userId, Guid tagId)
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
