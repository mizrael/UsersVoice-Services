using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using UsersVoice.Services.ContentAPI.CQRS.Queries;
using UsersVoice.Services.Infrastructure.Web;
using UsersVoice.Services.Infrastructure.Web.ActionResults;

namespace UsersVoice.Services.ContentAPI.ApiControllers
{
    /// <summary>
    /// Users API endpoint
    /// </summary>
    [RoutePrefix("api/useravatar")]
    public class UserAvatarController : ApiControllerBase
    {
         private readonly IMediator _mediator;

         public UserAvatarController(IMediator mediator)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");

            _mediator = mediator;
        }

        /// <summary>
        /// users avatar image endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri] Guid id)
        {
            var query = new UserAvatarQuery(id);
            var imagePath = await _mediator.SendAsync(query);

            return new FileResult(imagePath);
        }
    }
}
