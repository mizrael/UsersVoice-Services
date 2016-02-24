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
}
