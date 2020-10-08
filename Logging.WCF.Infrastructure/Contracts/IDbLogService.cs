
using Logging.WCF.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace Logging.WCF.Infrastructure.Contracts
{
    public interface ILogSinkerService : IDisposable
    {
        /// <summary>
        ///     TODO: This should be encrypted due to possible sensitive data being transmitted (for logging purposes)...
        /// </summary>
        /// <param name="loggingEventDto"></param>
        /// <returns></returns>
        Task<int?> SinkToLogAsync(LoggingEventDto loggingEventDto);
    }
}