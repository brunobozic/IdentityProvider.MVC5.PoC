using AutoMapper;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.ApplicationContext;
using IdentityProvider.Infrastructure.SessionStorageFactories;
using System;
using System.Web;
using System.Web.Mvc;

namespace IdentityProvider.Infrastructure.DatabaseLog
{
    public static class WcfLogFactory
    {
        private static IMapper _mapper;

        private static void InitializeWcfLogFactory(bool force)
        {
            if (!force)
            {
                if (LoggingStorageFactory<WcfAppenderService>.CreateStorageContainer().GetLogger() == null)
                    CreateWcfLogger();
            }
            else
            {
                CreateWcfLogger();
            }
        }

        private static void CreateWcfLogger()
        {
            try
            {
                IContextProvider contextService = null;
                IApplicationConfiguration configuration = null;
                WcfAppenderService loggingService = null;

                if (HttpContext.Current == null)
                    contextService = DependencyResolver.Current.GetService<ThreadContextService>();
                else
                    contextService = DependencyResolver.Current.GetService<HttpContextProvider>();

                configuration = DependencyResolver.Current.GetService<IApplicationConfiguration>();

                _mapper = DependencyResolver.Current.GetService<IMapper>();

                loggingService = new WcfAppenderService(configuration, _mapper);

                LoggingStorageFactory<WcfAppenderService>.CreateStorageContainer().Store(loggingService);
            }
            catch (Exception logEx)
            {
                throw new Exception(
                    "Exception while initializing and storing LoggingService instance into appropriate cache, detailed exc: " +
                    logEx);
            }
        }

        public static WcfAppenderService GetWcfLogger()
        {
            if (LoggingStorageFactory<WcfAppenderService>.CreateStorageContainer().GetLogger() == null)
                InitializeWcfLogFactory(true);

            return LoggingStorageFactory<WcfAppenderService>.CreateStorageContainer().GetLogger();
        }
    }
}