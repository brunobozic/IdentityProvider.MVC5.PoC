using System;
using IdentityProvider.Infrastructure.DatabaseLog.DTOs;

namespace IdentityProvider.Infrastructure.DatabaseLog
{
    public interface IDbLogService : IDisposable
    {
        /// <summary>
        ///     TODO: This should be encrypted due to possible sensitive data being transmitted (for logging purposes)...
        /// </summary>
        /// <param name="loggingEventDto"></param>
        /// <returns></returns>
        int? LogToDatabase(LoggingEventDto loggingEventDto);
    }
}