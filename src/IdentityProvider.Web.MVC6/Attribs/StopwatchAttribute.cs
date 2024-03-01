using IdentityProvider.Web.MVC6.AppConfiguration;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Diagnostics;

namespace IdentityProvider.Web.MVC6.Attribs
{
    public class StopWatchActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var httpContext = context.HttpContext;
            var stopwach = httpContext.Items[StopwatchResources.StopwachKey] as Stopwatch;
            stopwach.Stop();
            var time = stopwach.Elapsed;

            if (time.TotalSeconds > 5)
            {
                //var log = (ILogger) context.HttpContext.RequestServices.GetService(typeof(ILogger));
                //log.LogInformation($"{context.ActionDescriptor.DisplayName} execution time: {time}");
                Log.Information($"{context.ActionDescriptor.DisplayName} execution time: {time}");
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var stopwach = new Stopwatch();
            stopwach.Start();
            context.HttpContext.Items.Add(StopwatchResources.StopwachKey, stopwach);
        }
    }
}