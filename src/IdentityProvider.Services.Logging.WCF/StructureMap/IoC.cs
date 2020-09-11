using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.ApplicationContext;
using IdentityProvider.Infrastructure.DatabaseLog;
using IdentityProvider.Infrastructure.Email;
using IdentityProvider.Infrastructure.Logging.Log4Net;
using Module.Repository.EF;
using Module.Repository.EF.Repositories;
using Module.Repository.EF.UnitOfWorkInterfaces;
using StructureMap;

namespace HAC.Helpdesk.Services.Logging.WCF.StructureMap
{
    public static class Ioc
    {
        public static IContainer GetContainer()
        {
            return new Container(c =>
            {
                c.Scan(scan =>
                {
                    scan.WithDefaultConventions();
                });

                c.For<IApplicationConfiguration>().Use<ApplicationConfiguration>();

                // This is used for Log4Net Logger, contains http request context data to improve the detail level of logging
                c.For<IAddLoggingContextProvider>().Use<LoggingContextProvider>();

                // This is used for Log4Net Logger, contains http request context data to improve the detail level of logging
                c.For<ILog4NetLoggingService>().Use<Log4NetLoggingService>();

                // Email Service                 
                c.For<IEmailService>().Use<TextLoggingEmailService>();

                c.For<IDbLogService>().Use<DbLogService>();

                c.For<IUnitOfWorkAsync>().Use<UnitOfWork>();

                c.For(typeof(IRepositoryAsync<>)).Use(typeof(Repository<>));

                c.For<IWcfAppenderService>().Use<WcfAppenderService>();

                c.For<ILogWcf>().Use<LogWcfService>();
            });
        }
    }
}