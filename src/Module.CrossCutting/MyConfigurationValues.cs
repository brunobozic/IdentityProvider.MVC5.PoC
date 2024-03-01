using System.Net;

namespace Module.CrossCutting
{
    public class MyConfigurationValues : IMyConfigurationValues
    {
        public bool ResponseCaching { get; set; }
        public bool CorrelationIdEmission { get; set; }
        public bool SerilogElasticSearch { get; set; }
        public bool SerilogConsole { get; set; }
        public bool SerilogLofToFile { get; set; }
        public string GenericErrorMessageForEndUser { get; set; }

        public string InstanceName { get; set; }
        public string Environment { get; set; }
        public string ElasticsearchUrl { get; set; }
        public string CachingIsEnabled { get; set; }
        public string CacheTimeoutInSeconds { get; set; }

        public SerilogOptions Serilog { get; set; }
        public SmtpOptions SmtpOptions { get; set; }
        public string NotFoundErrorMessageForEndUser { get; set; }
        public int DefaultPageSize { get; set; } = 20;
        public int DefaultPageNumber { get; set; } = 1;
        public bool UseActiveDirectory { get; set; } = false;

        public string ActiveDirectoryIntegrationTurnedOff { get; set; } =
            "Active Directory integration is currently turned off.";

        public string Secret { get; set; }
        public long RefreshTokenTTL { get; set; }

        public string WorkerName { get; set; }
        public KafkaConsumerSettings KafkaConsumerSettings { get; set; }
        public KafkaProducerSettings KafkaProducerSettings { get; set; }
        public PollySettings PollySettings { get; set; }
        public DeadLetterArchiveJobSettings DeadLetterArchiveJobSettings { get; set; }
        public DeadLetterOutboxJobSettings DeadLetterOutboxJobSettings { get; set; }
        public int? MainKafkaMessagePollInterval { get; set; }
        public KafkaLoggingProducerSettings KafkaLoggingProducerSettings { get; set; }
    }

    public class SerilogOptions
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public MinimumLevel MinimumLevel { get; set; }
    }

    public class MinimumLevel
    {
        public string Default { get; set; }
        public Override Override { get; set; }
    }

    public class Override
    {
        public string Microsoft { get; set; }
        public string System { get; set; }
        public string MicrosoftAspNetCore { get; set; }
    }

    public class ServiceDisvoveryOptions
    {
        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }

    public class ConsulOptions
    {
        public string HttpEndpoint { get; set; }

        public DnsEndpoint DnsEndpoint { get; set; }
    }

    public class DnsEndpoint
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
    }
}