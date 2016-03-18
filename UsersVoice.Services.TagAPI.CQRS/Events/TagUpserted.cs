using System;
using MediatR;

namespace UsersVoice.Services.TagAPI.CQRS.Events
{
    public class TagUpserted : IAsyncNotification
    {
        public TagUpserted(Guid tagId)
        {
            if (tagId == Guid.Empty) throw new ArgumentNullException("tagId");
            TagId = tagId;
        }
        public Guid TagId { get; private set; }
    }
}
