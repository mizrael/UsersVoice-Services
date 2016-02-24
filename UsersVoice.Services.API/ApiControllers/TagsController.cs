using System;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using UsersVoice.Services.API.CQRS.Commands;

namespace UsersVoice.Services.API.ApiControllers
{
    [RoutePrefix("api/tags")]
    public class TagsController : ApiController
    { 
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]CreateTag command)
        {
            await _mediator.PublishAsync(command);
            return Ok();
        }
    }
}
