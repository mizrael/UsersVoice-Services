using MediatR;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.Common.CQRS.Queries
{
    public abstract class ArchiveQuery<TModel> : IAsyncRequest<PagedCollection<TModel>>, IQuery, IPagingInfo
    {
        protected ArchiveQuery(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}