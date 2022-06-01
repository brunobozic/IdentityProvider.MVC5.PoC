using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace IdentityProvider.Web.MVC6
{
    public class UnhandledExceptionMiddleware
    {
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        private readonly RequestDelegate next;

        public UnhandledExceptionMiddleware(ILogger<UnhandledExceptionMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                logger.LogError(exception,
                    $"Request {context.Request?.Method}: {context.Request?.Path.Value} failed");
                Log.Error(exception.Message, exception);
            }
        }
    }
}