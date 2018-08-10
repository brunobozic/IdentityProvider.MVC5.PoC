using System.Web;

namespace IdentityProvider.Infrastructure.GlobalAsaxHelpers
{
    public interface IGlobalAsaxHelpers
    {
        ControllerActionDto PopulateControllerRouteActionFromContext(HttpContext httpContext);
    }
}