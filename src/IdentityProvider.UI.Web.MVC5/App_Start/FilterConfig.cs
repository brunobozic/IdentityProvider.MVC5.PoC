

using System.Web.Mvc;

namespace IdentityProvider.UI.Web.MVC5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
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
                    new PerformanceLogActionFilter(performanceLogProvider, applicationConfiguration);

                filters.Add(performanceLogActionFilter);
            }
            catch (Exception)
            {
                // swallow up, non critical functionality
            }
        }
    }
}