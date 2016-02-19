using System;
using MediatR;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.ContentAPI.CQRS.Queries
{
    public class UserAvatarQuery : IAsyncRequest<string>, IQuery
    {
        public UserAvatarQuery(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException("userId");
            UserId = userId;
        }
        public Guid UserId { get; private set; }
    }
}
