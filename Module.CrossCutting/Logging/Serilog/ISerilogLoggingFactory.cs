using Serilog;

namespace IdentityProvider.Infrastructure.Logging.Serilog
{
    public interface ISerilogLoggingFactory
    {
        ILogger GetLogger(SerilogLogTypesEnum niasMessageAudit);
    }
}