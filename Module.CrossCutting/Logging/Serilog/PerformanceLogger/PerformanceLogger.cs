using System;

using IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog.Provider;

namespace IdentityProvider.Infrastructure.Logging.Serilog.PerformanceLogger
{
    public class PerformanceLogger : IPerformanceLogger
    {
        #region Ctor

        public PerformanceLogger( IPerformanceLogProvider logger)
        {
           
            _logger = logger;
        }

        #endregion Ctor

        public bool IsPerformanceLogEnabled()
        {
            var retVal = false;

            try
            {
                //retVal = _configurationProvider.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                //    "PerformanceLogging", false);
            }
            catch (Exception)
            {
            }

            return retVal;
        }

        #region Private Props

   
        private readonly IPerformanceLogProvider _logger;

        #endregion Private Props
    }
}