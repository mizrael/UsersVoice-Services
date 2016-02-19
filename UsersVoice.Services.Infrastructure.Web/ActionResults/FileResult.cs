using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace UsersVoice.Services.Infrastructure.Web.ActionResults
{
    public class FileResult : IHttpActionResult
    {
        private readonly string _filePath;
        private readonly string _contentType;

        public FileResult(string filePath, string contentType = null)
        {
            _filePath = filePath;

            var ext = Path.GetExtension(_filePath) ?? string.Empty;

            _contentType = string.IsNullOrWhiteSpace(contentType) ? MimeMapping.GetMimeMapping(ext) : contentType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var stream = File.OpenRead(_filePath);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType);

            return Task.FromResult(response);
        }

    }
}
