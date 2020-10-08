using log4net;
using log4net.Core;
using Logging.WCF.Infrastructure.Contracts;
using Logging.WCF.Models;
using Logging.WCF.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Logging.WCF.Services
{
    /// <summary>
    /// This class is a wrapper to enable programmatic configuration and access to a remote WCF service.
    /// As such it does not belong in the callee service layer but in the caller service/business layer.
    /// It is given here as an example of proper use.
    /// </summary>
    public class WCFLoggingManager : IWcfLoggingManager
    {
        private static ILogWcf _wcfLogService;

        public WCFLoggingManager(string url)
        {
            CreateChannelToWcfService(url);
        }

        private static void CreateChannelToWcfService(string url)
        {
            try
            {
                // address for service
                var address = new EndpointAddress(new Uri(url));

                // binding for service
                var binding = new BasicHttpBinding
                {
                    CloseTimeout = new TimeSpan(0, 0, 0, 30),
                    OpenTimeout = new TimeSpan(0, 0, 0, 30),
                    ReceiveTimeout = new TimeSpan(0, 0, 5, 0),
                    SendTimeout = new TimeSpan(0, 0, 5, 0),
                    TransferMode = TransferMode.Buffered
                };

                // get channel to wcf service from channelFactory class
                _wcfLogService = ChannelFactory<ILogWcf>.CreateChannel(binding, address);
            }
            catch (Exception exc)
            {
                // todo:
                ILog logger = LogManager.GetLogger(string.Empty);
                logger.Fatal(exc);
            }
        }


        public void Dispose()
        {
            if (_wcfLogService != null) _wcfLogService.Dispose();
        }

        public void LogToWCF(LoggingEvent loggingEvent)
        {
            var request = new LogToWCFServiceRequest();

            try
            {
                var dto = new LoggingEventDto
                {
                    Domain = loggingEvent.Domain,
                    UserName = loggingEvent.UserName,
                    TimeStamp = loggingEvent.TimeStamp,
                    ThreadName = loggingEvent.ThreadName,
                    // Repository = loggingEvent.Repository,
                    RenderedMessage = loggingEvent.RenderedMessage,
                    MessageObject = loggingEvent.MessageObject,
                    LoggerName = loggingEvent.LoggerName,
                    LocationInformation = loggingEvent.LocationInformation,
                    DisplayName = loggingEvent.Level.DisplayName,
                    Identity = loggingEvent.Identity,
                    Properties = loggingEvent.GetProperties(),
                    ExceptionObject = loggingEvent.ExceptionObject,
                    ExceptionString = loggingEvent.GetExceptionString()
                    // LoggingEventData = loggingEvent.GetLoggingEventData()
                };

                // send this string message to wcf service
                request.LoggingEventDto = dto;
                _wcfLogService.LogToWcfAsync(request);
            }
            catch (Exception exc)
            {
                //  "{"There was no endpoint listening at http://localhost:63247/LogWCF.svc that could accept the message. This is often caused by an incorrect address or SOAP action. See InnerException, if present, for more details."}"
                ILog logger = LogManager.GetLogger(string.Empty);
                logger.Fatal(exc);
            }
        }

        public List<LogToWCFServiceRequest> FakeList
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }

    public class FakeWcfAppenderService : IWcfLoggingManager
    {
        private readonly List<LogToWCFServiceRequest> _fakeList = new List<LogToWCFServiceRequest>();

        public FakeWcfAppenderService(string url)
        {

            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("WCFLoggingManager URL");

            CreateChannelToWcfService(url);
        }

        private static void CreateChannelToWcfService(string url)
        {
        }


        public void Dispose()
        {
            // todo:
            throw new NotImplementedException();
        }

        public void LogToWCF(LoggingEvent loggingEvent)
        {
            var request = new LogToWCFServiceRequest();

            try
            {
                var dto = new LoggingEventDto
                {
                    Domain = loggingEvent.Domain,
                    UserName = loggingEvent.UserName,
                    TimeStamp = loggingEvent.TimeStamp,
                    ThreadName = loggingEvent.ThreadName,
                    RenderedMessage = loggingEvent.RenderedMessage,
                    MessageObject = loggingEvent.MessageObject,
                    LoggerName = loggingEvent.LoggerName,
                    LocationInformation = loggingEvent.LocationInformation,
                    DisplayName = loggingEvent.Level.DisplayName,
                    Identity = loggingEvent.Identity,
                    Properties = loggingEvent.GetProperties(),
                    ExceptionObject = loggingEvent.ExceptionObject,
                    ExceptionString = loggingEvent.GetExceptionString()
                };

                // send this string message to wcf service
                request.LoggingEventDto = dto;

                if (FakeList == null)
                    FakeList = new List<LogToWCFServiceRequest>();

                FakeList.Add(request);
            }
            catch (Exception exc)
            {
                // todo:
                ILog logger = LogManager.GetLogger(string.Empty);
                logger.Fatal(exc);
            }
        }

        public List<LogToWCFServiceRequest> FakeList { get; set; }
    }
}