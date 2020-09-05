using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.ControllerAlertHelpers;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using StructureMap;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IdentityProvider.Controllers.Controllers
{
    public class BaseController : Controller, IController
    {
        private readonly ICookieStorageService _cookieStorageService;
        protected IErrorLogService _errorLogService;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private ICookieStorageService cookieStorageService;

        [DefaultConstructor]
        public BaseController(
            ICookieStorageService cookieStorageService
            , IErrorLogService errorLogService
            , IApplicationConfiguration applicationConfiguration
        )
        {
            _cookieStorageService = cookieStorageService;
            _errorLogService = errorLogService;
            _applicationConfiguration = applicationConfiguration;
        }

        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }

        public ViewResult HandleException(ExceptionContext filterContext, IErrorLogService errorLogService = null)
        {
            filterContext.ExceptionHandled = true;

            var exception = filterContext.Exception;

            if (_errorLogService == null)
                _errorLogService =
                    (RollingFileErrorLogProvider)DependencyResolver.Current.GetService(typeof(IErrorLogService));

            if (errorLogService == null) errorLogService = _errorLogService;

            errorLogService.LogFatal(this, exception.Message, exception);

            var viewResult = View("Error", new HandleErrorInfo(exception,
                filterContext.RouteData.Values["controller"].ToString(),
                filterContext.RouteData.Values["action"].ToString()));

            return viewResult;
        }
    }
}