using Module.CrossCutting;


namespace IdentityProvider.Web.MVC6.Controllers;

public interface IApplicationSettings
{
    bool ResponseCaching { get; set; }
    bool CorrelationIdEmission { get; set; }
    bool SerilogElasticSearch { get; set; }
    bool SerilogConsole { get; set; }
    bool SerilogLofToFile { get; set; }
    string GenericErrorMessageForEndUser { get; set; }
    string InstanceName { get; set; }
    string Environment { get; set; }
    string ElasticsearchUrl { get; set; }
    string CachingIsEnabled { get; set; }
    string CacheTimeoutInSeconds { get; set; }
    SerilogOptions Serilog { get; set; }
    SmtpOptions SmtpOptions { get; set; }

    string NotFoundErrorMessageForEndUser { get; set; }
    int DefaultPageSize { get; set; }
    int DefaultPageNumber { get; set; }

}