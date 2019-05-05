using System;
using System.Web.Mvc;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.Logging.Serilog.PerformanceLogger;
using IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog;
using IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog.Provider;

namespace IdentityProvider.UI.Web.MVC5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters( GlobalFilterCollection filters )
        {
            filters.Add(new HandleErrorAttribute());

            IPerformanceLogger performanceLogger = null;
            IApplicationConfiguration applicationConfiguration = null;

            try
            {
                performanceLogger = DependencyResolver.Current.GetService<IPerformanceLogger>();
                applicationConfiguration = DependencyResolver.Current.GetService<IApplicationConfiguration>();

                if (performanceLogger == null || !performanceLogger.IsPerformanceLogEnabled()) return;
                var performanceLogProvider = DependencyResolver.Current.GetService<IPerformanceLogProvider>();

                if (performanceLogProvider == null) return;
                var performanceLogActionFilter =
                    new PerformanceLogActionFilter(performanceLogProvider , applicationConfiguration);

                filters.Add(performanceLogActionFilter);
            }
            catch (Exception)
            {
                // swallow up, non critical functionality
            }
        }
    }
}