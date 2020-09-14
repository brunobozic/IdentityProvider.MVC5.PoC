using AutoMapper;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.ApplicationContext;
using IdentityProvider.Infrastructure.SessionStorageFactories;
using Logging.WCF.Models.Log4Net;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityProvider.Services.Log4Net
{
    public class Log4NetLogFactory
    {
        private static void InitializeLogFactory(bool force)
        {
            if (!force)
            {
                if (LoggingStorageFactory<ILog4NetLoggingService>.CreateStorageContainer().GetLogger() == null)
                    CreateLogger();
            }
            else
            {
                CreateLogger();
            }
        }

        public static void StoreLogger(ILog4NetLoggingService loggingService)
        {
            if (loggingService == null) throw new ArgumentException("LoggingService");
            LoggingStorageFactory<ILog4NetLoggingService>.CreateStorageContainer().Store(loggingService);
        }

        private static void CreateLogger()
        {
            try
            {
                if (HttpContext.Current == null)
                    DependencyResolver.Current.GetService<ThreadContextService>();
                else
                    DependencyResolver.Current.GetService<HttpContextProvider>();

                var configuration = DependencyResolver.Current.GetService<IApplicationConfiguration>();

                var loggingService = DependencyResolver.Current.GetService<ILog4NetLoggingService>();
                var mapper = DependencyResolver.Current.GetService<IMapper>();
                var addContextProvider = DependencyResolver.Current.GetService<IAddLoggingContextProvider>();
                var wcfAppenderService = new Log4NetLoggingService(configuration, addContextProvider);

                LoggingStorageFactory<ILog4NetLoggingService>.CreateStorageContainer().Store(loggingService);
            }
            catch (Exception logEx)
            {
                throw new Exception(
                    "Exception while initializing and storing LoggingService instance into appropriate cache, detailed exc: " +
                    logEx);
            }
        }

        public static ILog4NetLoggingService GetLogger()
        {
            if (LoggingStorageFactory<ILog4NetLoggingService>.CreateStorageContainer().GetLogger() == null)
                InitializeLogFactory(true);

            return LoggingStorageFactory<ILog4NetLoggingService>.CreateStorageContainer().GetLogger();
        }

        public static ILog4NetLoggingService GetLoggerForDbInterceptor()
        {
            if (LoggingStorageFactory<ILog4NetLoggingService>.CreateStorageContainer().GetLogger() == null)
                InitializeLogFactory(true);

            return LoggingStorageFactory<ILog4NetLoggingService>.CreateStorageContainer().GetLogger();
        }
    }
}
