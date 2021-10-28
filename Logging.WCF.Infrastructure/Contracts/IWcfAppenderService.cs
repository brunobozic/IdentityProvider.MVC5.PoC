using System;
using System.Collections.Generic;
using log4net.Core;
using Logging.WCF.Models;

namespace Logging.WCF.Infrastructure.Contracts
{
    public interface IWcfLoggingManager : IDisposable
    {
        List<LogToWCFServiceRequest> FakeList { get; set; }
        void LogToWCF(LoggingEvent loggingEvent);
    }
}