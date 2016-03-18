using System;
using MediatR;

namespace UsersVoice.Services.TagAPI.CQRS.Commands
{
    public class CreateIdeaTag : IAsyncNotification
    {
        public CreateIdeaTag(Guid tagId, Guid ideaId)
        {
            this.TagId = tagId;
            this.IdeaId = ideaId;
        }

        public Guid TagId { get; private set; }
        public Guid IdeaId { get; private set; }
    }
}