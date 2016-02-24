using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Commands
{
    public class CreateTag : IAsyncNotification
    {
        public CreateTag(Guid tagId, string text)
        {
            this.TagId = tagId;
            this.Text = text;
        }

        public Guid TagId { get; private set; }
        public string Text { get; private set; }
    }

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
