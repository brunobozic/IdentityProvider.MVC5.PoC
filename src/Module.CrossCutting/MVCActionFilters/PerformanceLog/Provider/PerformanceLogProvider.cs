using Module.CrossCutting.ApplicationContext;
using Module.CrossCutting.Logging.Serilog;
using Module.CrossCutting.MVCActionFilters.PerformanceLog.Model;
using Serilog;

namespace Module.CrossCutting.MVCActionFilters.PerformanceLog.Provider
{
    public class PerformanceLogProvider : IPerformanceLogProvider
    {
        private const string Pattern =
            "{CurrentEnvironment}{ApplicationId}{InstanceId}{Action}{Url}{Status}{StatusCode}{Browser}{Request}{Response}{Miliseconds}{CorrelationId}{Exception}";


        private readonly ILogger _loggingService;
        private IAddLoggingContextProvider _loggingContext;

        public PerformanceLogProvider(
            ISerilogLoggingFactory loggingFactory,
            IAddLoggingContextProvider addLoggingContext
        )
        {
            var loggingFactory1 = loggingFactory ?? throw new ArgumentNullException(nameof(loggingFactory));
            _loggingContext = addLoggingContext ?? throw new ArgumentNullException(nameof(addLoggingContext));


            _loggingService = loggingFactory1.GetLogger(SerilogLogTypesEnum.PerformanceLog);
        }

        public void Log(PerformanceLogTick t)
        {
            if (t.Stopwatch != null)
            {
                t.Stopwatch.Stop();

                _loggingService.Information(
                    Pattern,
                    //_applicationConfiguration.GetCurrentEnvironment().ToString(),
                    //_applicationConfiguration.GetApplicationId() ?? string.Empty,
                    //_applicationConfiguration.GetInstanceId() ?? string.Empty,
                    t.Action ?? string.Empty,
                    t.Url ?? string.Empty,
                    t.HttpResponse ?? string.Empty,
                    t.HttpResponseStatusCode ?? string.Empty,
                    t.Browser ?? string.Empty,
                    t.RequestJson,
                    t.ResponseJson,
                    t.Stopwatch.ElapsedMilliseconds,
                    t.CorrelationId,
                    t.Exception
                );
            }
            else
            {
                _loggingService.Information(
                    Pattern,
                    //_applicationConfiguration.GetCurrentEnvironment().ToString(),
                    //_applicationConfiguration.GetApplicationId() ?? string.Empty,
                    //_applicationConfiguration.GetInstanceId() ?? string.Empty,
                    t.Action ?? string.Empty,
                    t.Url ?? string.Empty,
                    t.HttpResponse ?? string.Empty,
                    t.HttpResponseStatusCode ?? string.Empty,
                    t.Browser ?? string.Empty,
                    t.RequestJson,
                    t.ResponseJson,
                    t.Miliseconds,
                    t.CorrelationId,
                    t.Exception
                );
            }
        }
    }
}