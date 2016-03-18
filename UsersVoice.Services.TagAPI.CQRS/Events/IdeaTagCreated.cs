using System;
using MediatR;

namespace UsersVoice.Services.TagAPI.CQRS.Events
{
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
}