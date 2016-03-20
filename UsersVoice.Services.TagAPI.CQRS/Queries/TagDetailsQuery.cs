using System;
using MediatR;
using UsersVoice.Services.Common.CQRS.Queries;
using UsersVoice.Services.TagAPI.CQRS.Queries.Models;

namespace UsersVoice.Services.TagAPI.CQRS.Queries
{
    public class TagDetailsQuery : IAsyncRequest<TagDetails>, IQuery
    {
        public Guid TagId { get; set; } 
        public string Slug { get; set; }
    }
}
