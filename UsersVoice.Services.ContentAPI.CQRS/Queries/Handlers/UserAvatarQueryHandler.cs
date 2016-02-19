using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;

namespace UsersVoice.Services.ContentAPI.CQRS.Queries.Handlers
{
    public class UserAvatarQueryHandler: IAsyncRequestHandler<UserAvatarQuery, string>
    {
        private readonly IEnumerable<string> _imagePaths;

        public UserAvatarQueryHandler(HttpContextBase httpContext)
        {
            if (httpContext == null) 
                throw new ArgumentNullException("httpContext");

            var basePath = httpContext.Server.MapPath("~/app_data/userAvatars/");
            _imagePaths = Directory.GetFiles(basePath, "*.jpg");
        }

        public Task<string> Handle(UserAvatarQuery query)
        {
            var imagePath = _imagePaths.OrderBy(i => Guid.NewGuid()).First();
            return Task.FromResult(imagePath);
        }
    }
}
