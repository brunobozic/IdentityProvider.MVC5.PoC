using IdentityProvider.Repository.EFCore;
using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using IdentityProvider.Repository.EFCore.EFDataContext;
using IdentityProvider.Web.MVC6.Controllers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Services;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;

namespace IdentityProvider.Web.MVC6
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
        {
            // Registers MediatR services
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            // This example registers two IMediator instances, which might be a mistake or a specific requirement.
            // Usually, you would have a single IMediator registration. If you need different behaviors,
            // consider using named or keyed registrations and resolve them specifically where needed.
            services.AddTransient<IMediator, Mediator>();

            // Assuming NoMediator is a custom implementation for a specific purpose, consider reviewing the need
            // and approach for registering multiple IMediator implementations as it might lead to confusion or errors.
            // If NoMediator serves a distinct purpose, consider using a different interface or service registration pattern.
            services.AddTransient<IMediator, NoMediator>();

            return services;
        }
        public static IServiceCollection AddCustomIdentityConfiguration(this IServiceCollection services, string connectionString)
        {
            // Configure DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.MigrationsAssembly("IdentityProvider.Repository.EFCore");
                }));

            // Add Identity
            services.AddIdentity<ApplicationUser, AppRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddRoleManager<RoleManager<AppRole>>()
                    .AddUserManager<UserManager<ApplicationUser>>()
                    .AddDefaultTokenProviders();

            // Configure Identity Options
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            // Additional services related to Identity or DbContext
            services.AddScoped<AppDbContext, AppDbContext>();
            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMyUnitOfWork, MyUnitOfWork>();
            services.AddTransient<IDomainEventsDispatcher, IntegrationEventDispatcher>();

            return services;
        }
        public static void RegisterRepositories(this IServiceCollection services)
        {
            var models = GetAllModels();

            foreach (var model in models)
            {
                var repositoryInterface = typeof(ITrackableRepository<>);
                repositoryInterface.MakeGenericType(model);
                var repositoryImplementation = typeof(TrackableRepository<>);
                repositoryImplementation.MakeGenericType(model);

                services.AddScoped(repositoryInterface, repositoryImplementation);
            }
        }

        public static void AddUrlHelper(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;

                return actionContext != null ? new UrlHelper(actionContext) : null;
            });
        }

        private static IEnumerable<Type> GetAllModels()
        {
            var assembly = Assembly.GetAssembly(typeof(ApplicationUser));
            var trackableInterface = typeof(ITrackable);

            return assembly.GetTypes().Where(t => trackableInterface.IsAssignableFrom(t)).ToList();
        }
        public static IServiceCollection ConfigureDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configure DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.MigrationsAssembly("IdentityProvider.Repository.EFCore");
                }));

            // Register DbContext as a service for DI
            services.AddScoped<AppDbContext, AppDbContext>();
            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped<IAppDbContext, AppDbContext>();

            // Additional data-related services
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMyUnitOfWork, MyUnitOfWork>();

            return services;
        }
        public static IEnumerable<Type> GetAllServiceTypes()
        {
            // Get the assembly where your services are defined
            Assembly assembly = Assembly.GetExecutingAssembly(); // You can also use Assembly.GetEntryAssembly() or specify another assembly

            // Find all types in the assembly that implement IService<T> or its derived interfaces
            var serviceTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IService<>).IsAssignableFrom(type));

            return serviceTypes;
        }
        public static void RegisterServices(this IServiceCollection services)
        {
            var serviceTypes = GetAllServiceTypes();

            foreach (var serviceType in serviceTypes)
            {
                var implementedInterfaces = serviceType.GetInterfaces();

                // Register each implemented interface with the corresponding service type
                foreach (var implementedInterface in implementedInterfaces)
                {
                    services.AddScoped(implementedInterface, serviceType);
                }
            }
        }

    }
}
