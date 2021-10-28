using System;
using System.Threading.Tasks;
using log4net;
using Logging.WCF.Infrastructure.Contracts;
using Logging.WCF.Models.DTOs;
using Logging.WCF.Repository.EF.ExtensionMethods;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Logging.WCF.Services.AvailableLogSinkers
{
    public class ElasticSearchSink : ILogSinkerService
    {
        private readonly ILogger _serilog;

        public ElasticSearchSink(ILogger serilog)
        {
            _serilog = serilog;

            var log = new LoggerConfiguration()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
                })
                .CreateLogger();

            _serilog = log;
        }

        public bool Disposed { get; set; }

        public void Dispose()
        {
            if (_serilog != null && !Disposed)
            {
                var d = (IDisposable) _serilog;
                d.Dispose();
                Disposed = true;
            }
        }

        public async Task<int?> SinkToLogAsync(LoggingEventDto loggingEventDto)
        {
            int? retVal = -1;

            var myLog = loggingEventDto.ConvertToDbLog();

            try
            {
                _serilog.Fatal("{0}", myLog);
            }
            catch (Exception dbException)
            {
                myLog.ExceptionMessage +=
                    string.Format("		[ DB ] ====>		Db exception while saving exception: [ {0} ]   [ {1} ]",
                        dbException.Message, dbException.InnerException);

                // LogToWCF to file...
                var logger = LogManager.GetLogger(GetType());
                logger.Fatal("", dbException);

                logger = null;

                Dispose();
            }

            return retVal;
        }
    }
}