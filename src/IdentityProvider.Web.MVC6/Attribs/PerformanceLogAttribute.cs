using AspectCore.DynamicProxy;
using System;

using System.Threading.Tasks;

namespace IdentityProvider.Web.MVC6
{
    public class PerformanceLogAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            Console.WriteLine("Invoking Aspect"); // We are in aspect
            await next(context); // Run the function
        }
    }
}