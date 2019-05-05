using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace IdentityProvider.Controllers.Authorize
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method , Inherited = true , AllowMultiple = true)]
    public class CustomAuthorizeByRole : AuthorizeAttribute
    {
        public CustomAuthorizeByRole( string role )
        {
            _role = role;
        }

        private string _role;

        protected override bool AuthorizeCore( HttpContextBase httpContext )
        {
            // Get the roles from the claims
            var identity = ( ClaimsIdentity ) httpContext.User.Identity;
            
            // Make sure they are authenticated
            if (!identity.IsAuthenticated)
                return false;

            var roles = identity.Claims.Where(claim => claim.Value == _role);

            // Check if they are authorized based on found claims
            if (identity.Claims.Any(i => i.Value == _role))
            {
                return true;
            }

            // Check if they are authorized
            // return RoleService.Authorize(_role , roles);

            return true;
        }
    }
}