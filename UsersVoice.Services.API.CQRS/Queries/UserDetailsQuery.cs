using System;
using MediatR;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class UserDetailsQuery : IAsyncRequest<UserDetails>, IQuery
    {
        public UserDetailsQuery(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException("userId");
            this.UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}