
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.ApplicationContext;
using IdentityProvider.Infrastructure.DatabaseLog;
using IdentityProvider.Infrastructure.Email;
using IdentityProvider.Infrastructure.Logging.Log4Net;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Repository.EF.EFDataContext;
using Module.Repository.EF;
using Module.Repository.EF.DataContextInterfaces;
using Module.Repository.EF.Repositories;
using Module.Repository.EF.SimpleAudit;
using Module.Repository.EF.UnitOfWorkInterfaces;
using StructureMap;
using StructureMap.Pipeline;

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
					scan.Assembly("HAC.Helpdesk.Services.Logging.WCF");
					scan.Assembly("HAC.Helpdesk.SimpleMembership.Repository.EF");
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

				c.For(typeof (IRepositoryAsync<>)).Use(typeof (Repository<>));

				c.For<IWcfAppenderService>().Use<WcfAppenderService>();

				c.For<ILogWcf>().Use<LogWcfService>();

				c.For<IAuditedDbContext<ApplicationUser>>()
					.LifecycleIs(new UniquePerRequestLifecycle())
					.Use(i => new AppDbContext("SimpleMembership"));


			});
		}
	}
}