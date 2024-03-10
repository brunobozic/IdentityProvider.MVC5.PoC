using Module.CrossCutting;

namespace IdentityProvider.Web.MVC6.Controllers
{
    public class ApplicationSettings : IApplicationSettings
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
}