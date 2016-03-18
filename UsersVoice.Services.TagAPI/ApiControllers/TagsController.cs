using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MediatR;
using UsersVoice.Services.Infrastructure.Common;
using UsersVoice.Services.Infrastructure.Web;
using UsersVoice.Services.TagAPI.CQRS.Commands;
using UsersVoice.Services.TagAPI.CQRS.Queries;
using UsersVoice.Services.TagAPI.CQRS.Queries.Models;

namespace UsersVoice.Services.TagAPI.ApiControllers
{
    [RoutePrefix("api/tags")]
    public class TagsController : ApiControllerBase
    { 
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");
            _mediator = mediator;
        }

        [HttpGet]
        [ResponseType(typeof(PagedCollection<TagArchiveItem>))]
        public async Task<IHttpActionResult> Get([FromUri]TagsArchiveQuery filter)
        {
            filter = filter ?? new TagsArchiveQuery();
            var items = await _mediator.SendAsync(filter);
            return OkOrNotFound(items);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]CreateTag command)
        {
            await _mediator.PublishAsync(command);
            return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostIdeaTag([FromBody]CreateIdeaTag command)
        {
            await _mediator.PublishAsync(command);
            return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostUserTag([FromBody]CreateUserTag command)
        {
            await _mediator.PublishAsync(command);
            return Ok();
        }
    }
}
