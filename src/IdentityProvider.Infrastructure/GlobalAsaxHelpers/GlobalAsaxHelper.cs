using AutoMapper;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IdentityProvider.Infrastructure.GlobalAsaxHelpers
{
    public class GlobalAsaxHelpers : IGlobalAsaxHelpers
    {
        private readonly IConfigurationProvider _configurationRepository;
        private string _currentAction;

        private string _currentController;

        public GlobalAsaxHelpers()
        {
            _configurationRepository =
                (IConfigurationProvider)DependencyResolver.Current.GetService(typeof(IConfigurationProvider));
            if (_configurationRepository == null) throw new ArgumentNullException(nameof(IConfigurationProvider));
        }


        public ControllerActionDto PopulateControllerRouteActionFromContext(HttpContext httpContext)
        {
            var returnValue = new ControllerActionDto();
            try
            {
                var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

                if (currentRouteData != null)
                {
                    if (currentRouteData.Values["controller"] != null &&
                        !string.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                        returnValue.CurrentController = currentRouteData.Values["controller"] + "Controller";

                    if (currentRouteData.Values["action"] != null &&
                        !string.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                        returnValue.CurrentAction = currentRouteData.Values["action"].ToString();
                }
            }
            catch (Exception ex)
            {
                // _logger?.("", ex);
            }

            return returnValue;
        }
    }
}