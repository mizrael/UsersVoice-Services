using System;
using MediatR;

namespace UsersVoice.Services.TagAPI.CQRS.Commands
{
    public class UpdateTag : IAsyncNotification
    {
        public UpdateTag(Guid tagId, string text)
        {
            this.TagId = tagId;
            this.Text = text;
        }

        public Guid TagId { get; private set; }
        public string Text { get; private set; }
    }
}