using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using IdentityProvider.Repository.EFCore;
using IdentityProvider.Repository.EFCore.EFDataContext;
using IdentityProvider.ServiceLayer.Services.ApplicationUserService;
using IdentityProvider.Web.MVC6;
using IdentityProvider.Web.MVC6.Controllers;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.CrossCutting.Domain;
using Quartz;
using Serilog;
using System;
using System.Diagnostics;
using System.Reflection;
using URF.Core.Abstractions.Services;
using URF.Core.Services;

public class Bootstrap
{
    public static IContainer Container { get; private set; }

    public static IContainer BuildContainer(string connStr, IServiceCollection services)
    {
        // create a builder
        var containerBuilder = new ContainerBuilder();
        // detect assembly name
        var executingAssembly = Assembly.GetExecutingAssembly();
        var path = Assembly.GetEntryAssembly().Location;
        // detect process module
        var processModule = Process.GetCurrentProcess().MainModule;
        // build a .net core native service provider, so we can later cross-wire it with AutoFac
        var builtServiceProvider = services.BuildServiceProvider();

        // Once you've registered everything in the ServiceCollection, call
        // Populate to bring those registrations into Autofac. This is
        // just like a foreach over the list of things in the collection
        // to add them to Autofac.
        containerBuilder.Populate(services);

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // read values from appsettings
        var config = new ConfigurationBuilder()
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                false) // beware this will default to Production appsettings if no ENV is defined on the OS                                                                                                                            // .AddJsonFile("appsettings.local.json", true) // load local settings (usually used for local debugging sessions)  ==> this will override all the other previously loaded appsettings, so comment this out in production!
                       //.AddJsonFile("appsettings.local.json", true)
                       //.SetBasePath(new FileInfo(processModule.FileName).DirectoryName) // this might fail on linux
                       //.SetBasePath(GetBasePath()) // this might fail on linux
                       //.SetBasePath("")
            .AddEnvironmentVariables()
            .Build();

        // Make your Autofac registrations. Order is important!
        // If you make them BEFORE you call Populate, then the
        // registrations in the ServiceCollection will override Autofac
        // registrations; if you make them AFTER Populate, the Autofac
        // registrations will override. You can make registrations
        // before or after Populate, however you choose.

        // bind appsetting values to a configuration class
        // make that class a singleton (application wide globally accessible, so we can access the same instance from everywhere)
        var settings = new ApplicationSettings();
        config.GetSection("ApplicationSettings").Bind(settings);
        containerBuilder.RegisterType<ApplicationSettings>().As<IApplicationSettings>().SingleInstance();
        containerBuilder.RegisterType<ApplicationSettings>().SingleInstance();
        containerBuilder.RegisterInstance(settings);

        #region Application services

        // => => => => => => Register your stuff here!

        containerBuilder.RegisterType(typeof(Mediator)).As(typeof(IMediator)).InstancePerLifetimeScope();

        containerBuilder.RegisterInstance(Log.Logger)
            .As<ILogger>()
            .SingleInstance();

        containerBuilder.RegisterType(typeof(UserService)).As(typeof(IUserService)).InstancePerLifetimeScope();
        //containerBuilder.RegisterType(typeof(ApplicationUserController)).As(typeof(IApplicationUserController))
        //    .InstancePerLifetimeScope();
        //containerBuilder.RegisterType<ApplicationUserController>().PropertiesAutowired();

        #endregion Application services

        #region Entity Framework

        containerBuilder.RegisterType(typeof(MyUnitOfWork)).As(typeof(IMyUnitOfWork)).InstancePerLifetimeScope();

        containerBuilder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();



        containerBuilder.RegisterType<StronglyTypedIdValueConverterSelector>()
            .As<IValueConverterSelector>()
            .InstancePerLifetimeScope();

        containerBuilder.Register(c =>
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            dbContextOptionsBuilder.UseSqlServer(connStr,
                x => x.MigrationsAssembly("IdentityProvider.Repository.EFCore"));
            dbContextOptionsBuilder
                .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
            dbContextOptionsBuilder.EnableSensitiveDataLogging(); // only for local development
            dbContextOptionsBuilder.EnableDetailedErrors(); // only for local development
            return new AppDbContext(dbContextOptionsBuilder.Options);
        })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope(); // within one lifetime, will keep getting the same instance, different lifetimes will resolve a different instance!

        #endregion Entity Framework

        #region MediatR

        containerBuilder.RegisterSource(new ScopedContravariantRegistrationSource(
            typeof(IRequestHandler<,>)
            , typeof(INotificationHandler<>)
        // , typeof(IValidator<>)
        ));

        var mediatrOpenTypes = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>)
            // typeof(IValidator<>),
        };

        containerBuilder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        containerBuilder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        containerBuilder.RegisterType<IntegrationEventDispatcher>().As<IDomainEventsDispatcher>()
            .InstancePerLifetimeScope();

        containerBuilder.RegisterAssemblyTypes(typeof(DomainEventBase).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(Module.CrossCutting.Domain.IIntegrationEvent<>))
            .InstancePerDependency();

        containerBuilder.RegisterGenericDecorator(
            typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
            typeof(INotificationHandler<>));

        containerBuilder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerDecorator<>),
            typeof(ICommandHandler<>));

        containerBuilder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>));

        containerBuilder.RegisterType<CommandsDispatcher>()
            .As<ICommandsDispatcher>()
            .InstancePerLifetimeScope();

        containerBuilder.RegisterType<CommandsScheduler>()
            .As<ICommandsScheduler>()
            .InstancePerLifetimeScope();

        #endregion MediatR

        #region Quartz

        // This section defines the *VERY IMPORTANT* job runners without which the app, such as it is (event driven) will not function
        // The code here basically runs background jobs that pick up internal commands and integration events from db tables and execute them
        // Therefore without these jobs, the db tables will keep filling up with rows (jobs) that will never get handled

        containerBuilder.RegisterAssemblyTypes(executingAssembly).Where(x => typeof(IJob).IsAssignableFrom(x))
            .InstancePerDependency();

        #endregion Quartz

        // Creating a new AutofacServiceProvider makes the container
        // available to your app using the Microsoft IServiceProvider
        // interface, so you can use those abstractions rather than
        // binding directly to Autofac.
        var builtContainer = containerBuilder.Build();

        Container = builtContainer;

        CommonServiceLocator.ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(builtContainer));

        // populate the composition root container so we can use this code later on to define scope (BeginLifetimeScope)
        CompositionRoot.SetContainer(builtContainer);

        return builtContainer;
    }
}