using Module.CrossCutting.MVCActionFilters.PerformanceLog.Provider;

namespace Module.CrossCutting.Logging.Serilog.PerformanceLogger
{
    public class PerformanceLogger : IPerformanceLogger
    {
        #region Private Props

        private readonly IPerformanceLogProvider _logger;

        #endregion Private Props

        #region Ctor

        public PerformanceLogger(IPerformanceLogProvider logger)
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
    }
}