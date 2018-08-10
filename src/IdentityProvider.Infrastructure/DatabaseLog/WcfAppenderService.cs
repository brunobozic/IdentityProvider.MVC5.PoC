using System;
using System.Collections.Generic;
using System.ServiceModel;
using AutoMapper;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.DatabaseLog.DTOs;
using IdentityProvider.Infrastructure.Logging.Log4Net;
using log4net.Core;

namespace IdentityProvider.Infrastructure.DatabaseLog
{
    // public class WcfAppenderService : AppenderSkeleton, IWcfAppenderService
    public class WcfAppenderService : IWcfAppenderService
    {
        private static ILogWcf _wcfLogService;
        private readonly IMapper _mapper;

        public WcfAppenderService(IApplicationConfiguration configurationRepository, IMapper mapper)
        {
            if (mapper != null) _mapper = mapper;
            if (configurationRepository != null)
            {
                // var url = configurationRepository.GetConfigurationValue<string>("LoggingServiceURL");
                var url = configurationRepository.GetWCFLoggingServiceURL();

                if (string.IsNullOrEmpty(url))
                    throw new ArgumentException("WcfAppenderService URL");

                CreateChannelToWcfService(url);
            }
        }


        public void AppendToLog(LoggingEvent loggingEvent)
        {
            Append(loggingEvent);
        }

        public List<LogToDatabaseRequest> FakeList
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
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
                Log4NetLoggingFactory.GetLogger()
                    .LogFatal(typeof(WcfAppenderService), "CreateChannelToWcfService", exc);
            }
        }

        private void Append(LoggingEvent loggingEvent)
        {
            // string output = string.IsNullOrEmpty(someString) ? "0" : someString;

            // if (_wcfLogService == null) CreateChannelToWcfService();

            var request = new LogToDatabaseRequest();

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
                    //LoggingEventData = loggingEvent.GetLoggingEventData()
                };

                // send this string message to wcf service
                request.LoggingEventDto = dto;
                _wcfLogService.AppendToLog(request);
            }
            catch (Exception exc)
            {
                //  "{"There was no endpoint listening at http://localhost:63247/LogWCF.svc that could accept the message. This is often caused by an incorrect address or SOAP action. See InnerException, if present, for more details."}"
                Log4NetLoggingFactory.GetLogger()
                    .LogFatal(typeof(WcfAppenderService), "Append(LoggingEvent loggingEvent)", exc);
            }
        }
    }


    public class FakeWcfAppenderService : IWcfAppenderService
    {
        private readonly List<LogToDatabaseRequest> _fakeList = new List<LogToDatabaseRequest>();

        public FakeWcfAppenderService(IApplicationConfiguration configurationRepository)
        {
            var configuration = configurationRepository;

            if (configuration == null)
                throw new ArgumentException("ConfigurationRepository");

            Url = configurationRepository.GetWCFLoggingServiceURL();

            if (string.IsNullOrEmpty(Url))
                throw new ArgumentException("WcfAppenderService URL");

            CreateChannelToWcfService(Url);
        }

        private string Url { get; }

        public void AppendToLog(LoggingEvent loggingEvent)
        {
            Append(loggingEvent);
        }

        public List<LogToDatabaseRequest> FakeList { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private static void CreateChannelToWcfService(string url)
        {
        }

        private void Append(LoggingEvent loggingEvent)
        {
            var request = new LogToDatabaseRequest();

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
                    FakeList = new List<LogToDatabaseRequest>();

                FakeList.Add(request);
            }
            catch (Exception exc)
            {
                Log4NetLoggingFactory.GetLogger()
                    .LogFatal(typeof(WcfAppenderService), "Append(LoggingEvent loggingEvent)", exc);
            }
        }
    }
}