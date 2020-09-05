using log4net.Core;
using System;
using System.Collections.Generic;

namespace IdentityProvider.Infrastructure.DatabaseLog
{
    public interface IWcfAppenderService : IDisposable
    {
        List<LogToDatabaseRequest> FakeList { get; set; }
        void AppendToLog(LoggingEvent loggingEvent);
    }
}