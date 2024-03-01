
using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.EFDataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;


namespace IdentityProvider.Web.MVC6.Extensions;

public static class ServiceCollectionExtension
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        var models = GetAllModels();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<DbContext, AppDbContext>();

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
}