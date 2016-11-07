using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace UsersVoice.Services.ContentAPI.CQRS.Queries.Handlers
{
    public class UserAvatarQueryHandler: IAsyncRequestHandler<UserAvatarQuery, string>
    {
        private readonly IEnumerable<string> _imagePaths;

        public UserAvatarQueryHandler(IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment == null) 
                throw new ArgumentNullException(nameof(hostingEnvironment));

            var contentRootPath = hostingEnvironment.ContentRootPath;
            var basePath = Path.Combine(contentRootPath, "assets\\userAvatars\\");

            _imagePaths = Directory.GetFiles(basePath, "*.jpg");
        }

        public Task<string> Handle(UserAvatarQuery query)
        {
            var imagePath = _imagePaths.OrderBy(i => Guid.NewGuid()).First();
            return Task.FromResult(imagePath);
        }
    }
}
