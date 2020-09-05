using IdentityProvider.Infrastructure.ConfigurationProvider;
using IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog.Provider;
using System;

namespace IdentityProvider.Infrastructure.Logging.Serilog.PerformanceLogger
{
    public class PerformanceLogger : IPerformanceLogger
    {
        #region Ctor

        public PerformanceLogger(IConfigurationProvider configurationProvider, IPerformanceLogProvider logger)
        {
            _configurationProvider = configurationProvider;
            _logger = logger;
        }

        #endregion Ctor

        public bool IsPerformanceLogEnabled()
        {
            var retVal = false;

            try
            {
                retVal = _configurationProvider.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "PerformanceLogging", false);
            }
            catch (Exception)
            {
            }

            return retVal;
        }

        #region Private Props

        private readonly IConfigurationProvider _configurationProvider;
        private readonly IPerformanceLogProvider _logger;

        #endregion Private Props
    }
}