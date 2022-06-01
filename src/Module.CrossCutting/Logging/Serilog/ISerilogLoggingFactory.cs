using Serilog;

namespace Module.CrossCutting.Logging.Serilog
{
    public interface ISerilogLoggingFactory
    {
        ILogger GetLogger(SerilogLogTypesEnum niasMessageAudit);
    }
}