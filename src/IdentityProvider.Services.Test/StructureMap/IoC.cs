using System.ComponentModel;
using HAC.Helpdesk.Infrastructure.ConfigurationFront.Implementation;
using HAC.Helpdesk.Infrastructure.ConfigurationFront.Interface;
using HAC.Helpdesk.Infrastructure.DatabaseLog;
using HAC.Helpdesk.Infrastructure.Email;
using HAC.Helpdesk.Infrastructure.Log4Net.Implementation;
using HAC.Helpdesk.Infrastructure.LoggingFront.Implementation;
using HAC.Helpdesk.Infrastructure.LoggingFront.Interface;
using HAC.Helpdesk.SimpleMembership.Repository.EF.Repositories;
using HAC.Helpdesk.SimpleMembership.Repository.EF.UnitOfWork;

namespace HAC.Helpdesk.SimpleMembership.Services.Test.StructureMap
{
    public static class Ioc
    {
        //public static IContainer GetContainer()
        //{
        //    return new Container(c =>
        //    {
        //        c.Scan(scan =>
        //        {
        //            scan.AssembliesFromApplicationBaseDirectory();
        //            scan.WithDefaultConventions();
        //        });


        //        c.For<IConfigurationRepository>().Use<ConfigFileConfigurationRepository>();

        //        // This is used for Log4Net Logger, contains http request context data to improve the detail level of logging
        //        c.For<IContextService>().Use<ThreadContextService>();

        //        // This is used for Log4Net Logger, contains http request context data to improve the detail level of logging
        //        c.For<ILoggingService>().Use<Log4NetLoggingService>();


        //        // Email Service                 
        //        c.For<IEmailService>().Use<TextLoggingEmailService>();


        //        c.For<ISimpleMembershipUnitOfWorkAsync>().Use<SimpleMembershipUnitOfWorkAsync>();

        //        c.For(typeof(ISimpleMembershipRepositoryAsync<>)).Use(typeof(SimpleMembershipRepositoryAsync<>));

        //        // x.For<ISimpleMembershipDataContext>().Use(i => new SimpleMembershipDataContext("DefaultConnection"));
        //        // x.For<HacEncDataContext>().Use(i => new HacEncDataContext("DefaultConnection"));


        //        //	c.For(typeof (IRepository<>)).Use(typeof (Repository<>));

        //        //x.For<ISmsMessagesRepository>().Use<SmsUlaznaPorukaRepository>();
        //        //x.For<ISmsMessagesArchiveRepository>().Use<SmsUlaznaPorukaArhivaRepository>();

        //        c.For<IWcfAppenderService>().Use<WcfAppenderService>();
        //    });
        //}
    }
}