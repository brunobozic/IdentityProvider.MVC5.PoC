using AutoMapper;
using IdentityProvider.Services.Logging.WCF;
using Logging.WCF.Infrastructure;
using Logging.WCF.Infrastructure.Contracts;
using Logging.WCF.Repository.EF.EFDataContext;
using Logging.WCF.Repository.EF.Repositories.DomainCore.RepositoryBaseImpl;
using Logging.WCF.Repository.EF.Repositories.DomainCore.RepositoryBaseImpl.RepositoryBaseInterfaces;
using Logging.WCF.Services;
using Microsoft.EntityFrameworkCore;
using Module.Repository.EF;
using Module.Repository.EF.RowLevelSecurity;
using Module.Repository.EF.UnitOfWorkInterfaces;
using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Linq;
using IConfigurationProvider = IdentityProvider.Services.Logging.WCF.IConfigurationProvider;

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

                // Email Service                 
                c.For<IEmailService>().Use<TextLoggingEmailService>();

                c.For<ILogSinkerService>().Use<DatabaseLogService>();

                c.For<IUnitOfWorkAsync>().Use<UnitOfWork>();

                c.For<IWcfLoggingManager>().Use<WCFLoggingManager>();
                c.For<IConfigurationProvider>().Use<ConfigFileConfigurationProvider>();
                c.For<ILogWcf>().Use<LogWcfService>();

                // c.For<DbContext>().Use(i => new MyAppDbContext()).LifecycleIs<UniquePerRequestLifecycle>();

                c.For<IRowAuthPoliciesContainer>().Use<RowAuthPoliciesContainer>();

                c.For<IConfigurationProvider>().Use<ConfigFileConfigurationProvider>();

                c.For(typeof(IEntityBaseRepositoryAsync<>)).Use(typeof(EntityBaseRepositoryAsync<>));
                c.For(typeof(IEntityBaseRepository<>)).Use(typeof(EntityBaseRepository<>));

                // c.For(typeof(MyAppDbContext)).Use(typeof(MyAppDbContext));

                // c.For<DbContext>().Use(i => new MyAppDbContext("SimpleMembership")).LifecycleIs<UniquePerRequestLifecycle>();
                c.For<DbContext>().Use(i => new MyAppDbContext(new DbContextOptions<MyAppDbContext>(), "Data Source=DESKTOP-OPEURE7\\SQLEXPRESS2; Integrated Security=True; MultipleActiveResultSets=True; Database=DatabaseLogger;")).LifecycleIs<UniquePerRequestLifecycle>();
                c.For<MyAppDbContext>().Use(i => new MyAppDbContext(new DbContextOptions<MyAppDbContext>(), "Data Source=DESKTOP-OPEURE7\\SQLEXPRESS2; Integrated Security=True; MultipleActiveResultSets=True; Database=DatabaseLogger;")).LifecycleIs<UniquePerRequestLifecycle>();

                // Get all Profiles
                var profiles = from t in typeof(Ioc).Assembly.GetTypes()
                               where typeof(Profile).IsAssignableFrom(t)
                               select (Profile)Activator.CreateInstance(t);

                // For each Profile, include that profile in the MapperConfiguration
                var config = new MapperConfiguration(cfg =>
                {
                    foreach (var profile in profiles)
                    {
                        cfg.AddProfile(profile);
                    }
                });

                // Create a mapper that will be used by the DI container
                var mapper = config.CreateMapper();

                c.For<IMapper>().Use(mapper);
            });
        }
    }
}