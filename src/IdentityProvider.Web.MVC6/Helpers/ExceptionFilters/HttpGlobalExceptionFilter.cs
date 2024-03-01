using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net;

namespace IdentityProvider.Web.MVC6.Helpers.ExceptionFilters;

public class HttpGlobalExceptionFilter : IExceptionFilter
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<HttpGlobalExceptionFilter> _logger;

    public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
    {
        _env = env;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        Log.Error(context.Exception, context.Exception.InnerException?.Message);

        _logger.LogError(new EventId(context.Exception.HResult),
            context.Exception,
            "{0}");

        if (context.Exception.GetType() == typeof(DomainException))
        {
            var json = new JsonErrorResponse
            {
                Messages = new[] { context.Exception.Message }
            };

            context.Result = new BadRequestObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        else
        {
            var json = new JsonErrorResponse
            {
                Messages = new[]
                    { "Oops, an error occured. Trace Identifier: " + context.HttpContext?.TraceIdentifier ?? "N/A" }
            };

            if (_env.EnvironmentName == EnvironmentName.Development ||
                _env.EnvironmentName.ToUpper() == "LocalDevelopment".ToUpper())
                json.DeveloperMessage = context.Exception;

            context.Result = new InternalServerErrorObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        context.ExceptionHandled = true;
    }

    private class JsonErrorResponse
    {
        public string[] Messages { get; set; }

        public object DeveloperMessage { get; set; }
    }
}