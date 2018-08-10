using System;
using System.Linq;
using AutoMapper;
using StructureMap;

namespace HAC.Helpdesk.SimpleMembership.Repository.EF.Test.StructureMap
{
    internal class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            var profiles = from t in typeof(DefaultRegistry).Assembly.GetTypes()
                where typeof(Profile).IsAssignableFrom(t)
                select (Profile) Activator.CreateInstance(t);

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });

            var mapper = config.CreateMapper();

            For<MapperConfiguration>().Use(config);
            For<IMapper>().Use(mapper);

            RegisterRepositories(mapper);
        }

        private void RegisterRepositories(IMapper mapper)
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory();
                scan.WithDefaultConventions();
            });

            For<IConfigurationRepository>().Use<ConfigFileConfigurationRepository>().Ctor<IMapper>().Is(mapper);


            // This is used for Log4Net Logger, contains http request context data to improve the detail level of logging
            For<IContextService>().Use<ThreadContextService>().Ctor<IMapper>().Is(mapper);


            // This is used for Log4Net Logger, contains http request context data to improve the detail level of logging
            For<ILoggingService>().Use<Log4NetLoggingService>().Ctor<IMapper>().Is(mapper);


            // Email Service                 
            For<IEmailService>().Use<TextLoggingEmailService>().Ctor<IMapper>().Is(mapper);


            For<ISimpleMembershipUnitOfWorkAsync>().Use<SimpleMembershipUnitOfWorkAsync>().Ctor<IMapper>().Is(mapper);


            For(typeof(ISimpleMembershipRepositoryAsync<>))
                .Use(typeof(SimpleMembershipRepositoryAsync<>))
                .Ctor<IMapper>()
                .Is(mapper);


            // x.For<ISimpleMembershipDataContext>().Use(i => new SimpleMembershipDataContext("DefaultConnection"));
            // x.For<HacEncDataContext>().Use(i => new HacEncDataContext("DefaultConnection"));
            //	c.For(typeof (IRepository<>)).Use(typeof (Repository<>));
            //x.For<ISmsMessagesRepository>().Use<SmsUlaznaPorukaRepository>();
            //x.For<ISmsMessagesArchiveRepository>().Use<SmsUlaznaPorukaArhivaRepository>();

            For<IWcfAppenderService>().Use<WcfAppenderService>().Ctor<IMapper>().Is(mapper);
        }

        #endregion
    }
}