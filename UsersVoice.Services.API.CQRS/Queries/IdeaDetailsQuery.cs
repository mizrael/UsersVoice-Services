using System;
using MediatR;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class IdeaDetailsQuery : IAsyncRequest<IdeaDetails>, IQuery
    {
        public IdeaDetailsQuery(Guid ideaId)
        {
            if (ideaId == Guid.Empty)
                throw new ArgumentNullException("ideaId");
            this.IdeaId = ideaId;
        }

        public Guid IdeaId { get; private set; }
    }
}