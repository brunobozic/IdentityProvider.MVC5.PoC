using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace IdentityProvider.Controllers.Helpers
{
    public class ChallengeResult : HttpUnauthorizedResult
    {
        // Used for XSRF protection when adding external logins

        private const string XsrfKey = "XsrfId";
        private string v;

        public ChallengeResult(string statusDescription, string v) : base(statusDescription)
        {
            this.v = v;
        }


        public ChallengeResult(string provider, string redirectUri, string userId)
        {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
        }

        private string LoginProvider { get; }
        private string RedirectUri { get; }
        private string UserId { get; }

        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
            if (UserId != null)
                properties.Dictionary[XsrfKey] = UserId;
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        }
    }
}