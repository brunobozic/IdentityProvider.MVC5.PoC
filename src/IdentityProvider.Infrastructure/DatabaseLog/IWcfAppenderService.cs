using System;
using System.Collections.Generic;
using log4net.Core;

namespace IdentityProvider.Infrastructure.DatabaseLog
{
    public interface IWcfAppenderService : IDisposable
    {
        List<LogToDatabaseRequest> FakeList { get; set; }
        void AppendToLog(LoggingEvent loggingEvent);
    }
}