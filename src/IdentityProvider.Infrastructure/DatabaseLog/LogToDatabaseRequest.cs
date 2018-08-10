using IdentityProvider.Infrastructure.DatabaseLog.DTOs;

namespace IdentityProvider.Infrastructure.DatabaseLog
{
    public class LogToDatabaseRequest
    {
        public LoggingEventDto LoggingEventDto { get; set; }
    }
}