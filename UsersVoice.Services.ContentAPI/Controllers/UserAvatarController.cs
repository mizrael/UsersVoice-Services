using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UsersVoice.Services.ContentAPI.CQRS.Queries;

namespace UsersVoice.Services.ContentAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserAvatarController : Controller
    {
        private readonly IMediator _mediator;

        public UserAvatarController(IMediator mediator)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");

            _mediator = mediator;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new UserAvatarQuery(id);
            var imagePath = await _mediator.SendAsync(query);
            var imageData = System.IO.File.ReadAllBytes(imagePath);
            return File(imageData, "image/jpg");
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
