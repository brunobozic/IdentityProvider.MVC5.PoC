using System;

namespace IdentityProvider.Web.MVC6.Controllers
{
    internal class HandleErrorInfo
    {
        private Exception exception;
        private string controllerName;
        private string actionName;

        public HandleErrorInfo(Exception exception, string controllerName, string actionName)
        {
            this.exception = exception;
            this.controllerName = controllerName;
            this.actionName = actionName;
        }
    }
}