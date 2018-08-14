using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IdentityProvider.Controllers;
using IdentityProvider.Controllers.Controllers;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.ApplicationContext;
using IdentityProvider.Infrastructure.Caching;
using IdentityProvider.Infrastructure.ConfigurationProvider;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Email;
using IdentityProvider.Infrastructure.GlobalAsaxHelpers;
using IdentityProvider.Infrastructure.Logging.Serilog;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Infrastructure.URLConfigHelpers;
using IdentityProvider.Models.ViewModels.Error;
using IdentityProvider.UI.Web.MVC5.DependencyResolution;
using Microsoft.Owin.Security;

namespace IdentityProvider.UI.Web.MVC5
{
    public class MvcApplication : HttpApplication
    {
        private IApplicationConfiguration _applicationConfiguration;
        private IConfigurationProvider _configurationRepository;
        private ControllerActionDto _controllerRouteAction;
        private ICookieProvider _cookieProvider;
        private ICookieStorageService _cookieStorage;
        private IGlobalAsaxHelpers _helper;
        private IErrorLogService _logger;
        private ISerilogLoggingFactory _loggingFactory;
        private ISessionCacheProvider _sessionCacheProvider;

        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperBootStrapper.ConfigureAutoMapper();

            // Services.AutoMapper.AutoMapperBootStrapper.ConfigureAutoMapper();

            EmailServiceFactory.InitializeEmailServiceFactory
                (DependencyResolver.Current.GetService<IEmailService>());

            ControllerBuilder.Current.SetControllerFactory(new IoCControllerFactory());

            _logger = DependencyResolver.Current.GetService<IErrorLogService>();
            _logger.LogInfo(this , $"Application start: [ {DateTime.Now} ]");
        }


        private void BootstrapGlobalAsax()
        {
            _configurationRepository =
                ( IConfigurationProvider ) DependencyResolver.Current.GetService(typeof(IConfigurationProvider));
            _cookieProvider = ( ICookieProvider ) DependencyResolver.Current.GetService(typeof(ICookieProvider));

            _helper = ( IGlobalAsaxHelpers ) DependencyResolver.Current.GetService(typeof(IGlobalAsaxHelpers));

            if (_configurationRepository == null) throw new ArgumentNullException(nameof(IConfigurationProvider));
            if (_cookieProvider == null) throw new ArgumentNullException(nameof(ICookieProvider));
            if (_helper == null) throw new ArgumentNullException(nameof(IGlobalAsaxHelpers));

            _applicationConfiguration =
                ( IApplicationConfiguration ) DependencyResolver.Current.GetService(typeof(IApplicationConfiguration));

            // store the application configuration object in session cache (for the application scope) ...
            _sessionCacheProvider.SaveForApplication(_applicationConfiguration , "ApplicationConfiguration");


            _loggingFactory =
                ( ISerilogLoggingFactory ) DependencyResolver.Current.GetService(typeof(ISerilogLoggingFactory));
            var loggingContext = new LoggingContextProvider();
            _logger = new SerilogErrorLogProvider(_loggingFactory , loggingContext);
        }


        private HttpContext ExtractContextFromSender( object sender )
        {
            HttpContext returnValue = null;

            var mySender = sender as MvcApplication;

            if (mySender != null)
                returnValue = mySender.Context;

            return returnValue;
        }

        protected void Application_BeginRequest( object sender , EventArgs e )
        {
            // unfortunately, this one is needed everywhere
            _helper = ( IGlobalAsaxHelpers ) DependencyResolver.Current.GetService(typeof(IGlobalAsaxHelpers));

            _controllerRouteAction =
                _helper?.PopulateControllerRouteActionFromContext(ExtractContextFromSender(sender));
        }

        protected void Application_EndRequest( object sender , EventArgs e )
        {
            _controllerRouteAction =
                _helper?.PopulateControllerRouteActionFromContext(ExtractContextFromSender(sender));
        }

        private void Application_PreRequestHandlerExecute( object sender , EventArgs e )
        {
            _controllerRouteAction = _helper.PopulateControllerRouteActionFromContext(ExtractContextFromSender(sender));
        }

        protected void Application_Error( object sender , EventArgs e )
        {
            #region requires this way of error handling

            try
            {
                if (_logger == null)
                    _logger = DependencyResolver.Current.GetService<IErrorLogService>();

                _helper = DependencyResolver.Current.GetService<IGlobalAsaxHelpers>();
                _cookieStorage = DependencyResolver.Current.GetService<ICookieStorageService>();
            }
            catch (Exception exception)
            {
                // _logger.Error("",exception);
            }

            _controllerRouteAction = _helper.PopulateControllerRouteActionFromContext(ExtractContextFromSender(sender));

            var ex = Server.GetLastError();

            var controller = new ErrorController(_cookieStorage);
            var routeData = new RouteData();

            var httpContext = ExtractContextFromSender(sender);
            if (ex != null)
                if (ex is HttpException)
                {
                    var httpEx = ex as HttpException;

                    _logger?.LogFatal(this , "=================================================");
                    _logger?.LogFatal(this ,
                        $"{DateTime.UtcNow}: HTTP error [ {httpEx.GetHttpCode()} ] at [ {_controllerRouteAction.CurrentController}/{_controllerRouteAction.CurrentAction} ] {Environment.NewLine} [ {ex} {Environment.NewLine} ]");
                    _logger?.LogFatal(this , "");
                }
                else
                {
                    _logger?.LogFatal(this , "=================================================");
                    _logger?.LogFatal(this ,
                        $"{DateTime.UtcNow}: error '{ex.Message}' at {_controllerRouteAction.CurrentController}/{_controllerRouteAction.CurrentAction} {Environment.NewLine} [ {ex} {Environment.NewLine}  ]");
                    _logger?.LogFatal(this , "");
                }

            if (_controllerRouteAction.CurrentController.Equals("Account") &&
                _controllerRouteAction.CurrentAction.Equals("Login") && ( ( HttpException ) ex ).GetHttpCode() == 500 &&
                ex.Message.Contains("anti-forgery"))
                Response.Redirect(UrlConfigHelper.GetRoot() + "Account/Login" , true);

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException ? ( ( HttpException ) ex ).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;

            routeData.Values[ "controller" ] = "Error";
            routeData.Values[ "action" ] = "Error";

            controller.ViewData.Model = new ErrorViewModel(ex , _controllerRouteAction.CurrentController ,
                _controllerRouteAction.CurrentAction);

            ( ( IController ) controller ).Execute(new RequestContext(new HttpContextWrapper(httpContext) , routeData));

            #endregion requires this way of error handling
        }

        #region Global.asax helpers

        private IAuthenticationManager AuthenticationManager => Context.GetOwinContext().Authentication;

        private bool CurrentUserAuthenticated => Context.GetOwinContext().Authentication.User.Identity.IsAuthenticated;

        #endregion Global.asax helpers
    }
}