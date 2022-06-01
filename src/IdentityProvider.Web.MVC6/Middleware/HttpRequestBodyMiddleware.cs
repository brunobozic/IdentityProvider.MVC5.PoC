using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace IdentityProvider.Web.MVC6
{
    public class HttpRequestBodyMiddleware
    {
        private readonly ILogger logger;
        private readonly RequestDelegate next;

        public HttpRequestBodyMiddleware(ILogger<HttpRequestBodyMiddleware> logger,
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            var reader = new StreamReader(context.Request.Body);

            string body = await reader.ReadToEndAsync();
            logger.LogInformation(
                $"Request {context.Request?.Method}: {context.Request?.Path.Value}\n{body}");

            context.Request.Body.Position = 0L;

            await next(context);
        }
    }
}