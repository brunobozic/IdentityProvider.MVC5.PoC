using System.ComponentModel.Design;
using IdentityProvider.Infrastructure;
using IdentityProvider.Infrastructure.ApplicationContext;
using IdentityProvider.Infrastructure.Certificates.ExpiryValidation;
using IdentityProvider.Infrastructure.Certificates.FromEmbeddedResource;
using IdentityProvider.Infrastructure.Certificates.Manager;
using IdentityProvider.Infrastructure.ConfigurationProvider;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.DatabaseAudit;
using IdentityProvider.Infrastructure.GlobalAsaxHelpers;
using IdentityProvider.Infrastructure.Logging.Log4Net;
using IdentityProvider.Infrastructure.Logging.Serilog;
using IdentityProvider.Infrastructure.Logging.Serilog.AuditLog;
using IdentityProvider.Infrastructure.Logging.Serilog.PerformanceLogger;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Services;
using IdentityProvider.Services.ApplicationRoleService;
using IdentityProvider.Services.AuditTrailService;
using IdentityProvider.Services.OperationsService;
using IdentityProvider.Services.ResourceService;
using IdentityProvider.Services.RowLeveLSecurityUserGrantService;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Module.ServicePattern;
using StructureMap.Pipeline;

namespace IdentityProvider.UI.Web.MVC5.DependencyResolution
{
    using AutoMapper;
    using IdentityProvider.Infrastructure.ApplicationConfiguration;
    using IdentityProvider.Infrastructure.Caching;
    using IdentityProvider.Infrastructure.Certificates.FromStore;
    using IdentityProvider.Infrastructure.DatabaseLog;
    using IdentityProvider.Infrastructure.Email;
    using IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog.Provider;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Module.Repository.EF;
    using Module.Repository.EF.Repositories;
    using Module.Repository.EF.UnitOfWorkInterfaces;
    using StructureMap;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;

    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        private readonly DpapiDataProtectionProvider _provider = new DpapiDataProtectionProvider("IdentityProvider");

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssembliesAndExecutablesFromApplicationBaseDirectory();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });

            //For<IDataContextAsync>()
            //    .Use<DataContextAsync>()
            //    .Ctor<HAC.Helpdesk.SimpleMembership.Repository.EF.EFDataContext.IdentityDbContext>()
            //    .Is<HAC.Helpdesk.SimpleMembership.Repository.EF.EFDataContext.IdentityDbContext>((SmartInstance<HAC.Helpdesk.SimpleMembership.Repository.EF.EFDataContext.IdentityDbContext, HAC.Helpdesk.SimpleMembership.Repository.EF.EFDataContext.IdentityDbContext> cfg) => cfg.SelectConstructor(() => new HAC.Helpdesk.SimpleMembership.Repository.EF.EFDataContext.IdentityDbContext("nameOrConnectionString"))
            //    .Ctor<string>()
            //    .Is("SimpleMembership"));

            For<Microsoft.Owin.Security.IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);

            For<IUserStore<ApplicationUser>>()
                .Use<UserStore<ApplicationUser>>()
                .Ctor<DbContext>()
                .Is<Repository.EF.EFDataContext.AppDbContext>((SmartInstance<Repository.EF.EFDataContext.AppDbContext, DbContext> cfg) => cfg.SelectConstructor(() => new Repository.EF.EFDataContext.AppDbContext("connectionStringName"))
                .Ctor<string>()
                .Is("SimpleMembership"));

            ForConcreteType<UserManager<IdentityUser>>()
                .Configure
                .SetProperty(userManager => userManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = true,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true
                })
                .SetProperty(userManager => userManager.UserValidator = new UserValidator<IdentityUser>(userManager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                })
                .SetProperty(userManager => userManager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser, string>(_provider.Create("ASP.NET Identity")))
                .SetProperty(userManager => userManager.UserLockoutEnabledByDefault = true)
                .SetProperty(userManager => userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5))
                .SetProperty(userManager => userManager.MaxFailedAccessAttemptsBeforeLockout = 5)
                .SetProperty(userManager => userManager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<IdentityUser>
                {
                    MessageFormat = "Your security code is {0}"
                }))
                .SetProperty(userManager => userManager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<IdentityUser>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is {0}"
                }))
                .SetProperty(userManager => userManager.EmailService = new EmailService())
                //.SetProperty(userManager => userManager.SmsService = new SmsService())

                ;

            //ForConcreteType<DataContextAsync>()
            //    .Configure
            //    .SelectConstructor(() => new DataContextAsync("connection string ctor"))
            //    .Ctor<string>()
            //    .Is("SimpleMembership");

            For<IMemoryCacheProvider>().Use<MemoryCacheProvider>().LifecycleIs<UniquePerRequestLifecycle>();

            For<IdentityProvider.Infrastructure.ConfigurationProvider.IConfigurationProvider>().Use<ConfigFileConfigurationProvider>().LifecycleIs<UniquePerRequestLifecycle>();

            For<IAddLoggingContextProvider>().Use<LoggingContextProvider>().LifecycleIs<UniquePerRequestLifecycle>();

            For<ISerilogLoggingFactory>().Use<LoggingFactory>().LifecycleIs<UniquePerRequestLifecycle>();
            For<ICertificateFromStoreProvider>().Use<CertificateFromStoreProvider>().LifecycleIs<UniquePerRequestLifecycle>();
            For<IAuditLogService>().Use<SerilogAuditLogProvider>().LifecycleIs<UniquePerRequestLifecycle>();
            For<IErrorLogService>().Use<SerilogErrorLogProvider>().LifecycleIs<UniquePerRequestLifecycle>();
            For<ICertificateManager>().Use<CertificateManager>().LifecycleIs<UniquePerRequestLifecycle>();
            For<ICertificateFromEmbededResourceProvider>().Use<CertificateFromEmbeddedResourceProvider>().LifecycleIs<UniquePerRequestLifecycle>();
            For<ICertificateExpirationValidator>().Use<CertificateExpirationValidator>().LifecycleIs<UniquePerRequestLifecycle>();
            For<ICachedUserAuthorizationGrantsProvider>().Use<CachedUserAuthorizationGrantsProvider>().LifecycleIs<UniquePerRequestLifecycle>();
            // ================================================================================

            For<IApplicationRoleService>().Use<ApplicationRoleService>().LifecycleIs<UniquePerRequestLifecycle>();
            For<IOperationService>().Use<OperationsService>().LifecycleIs<UniquePerRequestLifecycle>();
            For<IApplicationResourceService>().Use<ApplicationResourceService>().LifecycleIs<UniquePerRequestLifecycle>();
            For<IAuditTrailService>().Use<AuditTrailService>().LifecycleIs<UniquePerRequestLifecycle>();
            For<IRepositoryAsync<DbAuditTrail>>().Use<Repository<DbAuditTrail>>().LifecycleIs<UniquePerRequestLifecycle>();
            // For(typeof(IService<>)).Use(typeof(Service<>));
            For<ICachedUserAuthorizationGrantsProvider>().Use<CachedUserAuthorizationGrantsProvider>().LifecycleIs<UniquePerRequestLifecycle>();
            // ================================================================================
            For<IAuditTrailService>().Use<AuditTrailService>().LifecycleIs<UniquePerRequestLifecycle>();
            For<IRepositoryAsync<DbAuditTrail>>().Use<Repository<DbAuditTrail>>().LifecycleIs<UniquePerRequestLifecycle>();

            //For<IApplicationConfiguration>()
            //    .Use<ApplicationConfiguration>()
            //    //.Ctor<ApplicationConfiguration>()
            //    //.Is<ApplicationConfiguration>(ac => ac.SelectConstructor(() =>
            //    //    new ApplicationConfiguration(Infrastructure.ConfigurationProvider.IConfigurationProvider, true))
            //    ;

            For<IPerformanceLogProvider>().Use<PerformanceLogProvider>().LifecycleIs<UniquePerRequestLifecycle>();
            For<IPerformanceLogger>().Use<PerformanceLogger>().LifecycleIs<UniquePerRequestLifecycle>();

            // This is how we pass a primitive value into class constructor
            //For<IAuditedDbContext<ApplicationUser>>().Use(i => new IdentityProvider.Repository.EF.EFDataContext.AppDbContext("SimpleMembership")).LifecycleIs<UniquePerRequestLifecycle>();

            For<DbContext>().Use(i => new IdentityProvider.Repository.EF.EFDataContext.AppDbContext("SimpleMembership")).LifecycleIs<UniquePerRequestLifecycle>();

            For(typeof(IRoleStore<ApplicationRole, string>)).Use(typeof(RoleStore<ApplicationRole>))
            .Ctor<DbContext>()
            .Is<Repository.EF.EFDataContext.AppDbContext>((SmartInstance<Repository.EF.EFDataContext.AppDbContext, DbContext> cfg) => cfg.SelectConstructor(() => new Repository.EF.EFDataContext.AppDbContext("connectionStringName"))
            .Ctor<string>()
            .Is("SimpleMembership"));

            For<IUnitOfWorkAsync>().Use<UnitOfWork>().LifecycleIs<UniquePerRequestLifecycle>();
            For(typeof(IRepositoryAsync<>)).Use(typeof(Repository<>));
            For<ICookieStorageService>().Use<CookieStorageService>().LifecycleIs<UniquePerRequestLifecycle>();

            For<IEmailService>().Use<TextLoggingEmailService>().LifecycleIs<UniquePerRequestLifecycle>();

            For<IIdentityMessageService>().Use<GmailEmailService>().LifecycleIs<UniquePerRequestLifecycle>();

            For<ILog4NetLoggingService>().Use<Log4NetLoggingService>().LifecycleIs<UniquePerRequestLifecycle>();
            For<IContextProvider>().Use<HttpContextProvider>().LifecycleIs<UniquePerRequestLifecycle>();
            For<IApplicationConfiguration>().Use<ApplicationConfiguration>().LifecycleIs<UniquePerRequestLifecycle>();
            For<IGlobalAsaxHelpers>().Use<GlobalAsaxHelpers>().LifecycleIs<UniquePerRequestLifecycle>();


            // WCF based logging sink (endpoint)
            For<IWcfAppenderService>().Use<WcfAppenderService>().LifecycleIs<UniquePerRequestLifecycle>();
            // For<ILogWcf>().Use<LogWcf>();


            // Identity 2.0 Facade
            For<IWebSecurity>().Use<WebSecurity>().LifecycleIs<UniquePerRequestLifecycle>();

            // Get all Profiles
            var profiles = from t in typeof(DefaultRegistry).Assembly.GetTypes()
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

            // Register the DI interfaces with their implementation
            For<IConfigurationProvider>().Use(config);
            For<IMapper>().Use(mapper);
        }

        #endregion
    }
}