
using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.EFDataContext;
using IdentityProvider.ServiceLayer.Services.ApplicationUserService;
using Microsoft.EntityFrameworkCore;
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


namespace IdentityProvider.Web.MVC6.Helpers;

public static class RegisterTrackableRepositories
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

    private static IEnumerable<Type> GetAllMarkedServices()
    {
        var assembly = Assembly.GetAssembly(typeof(UserService));
        var trackableInterface = typeof(IService<>);
        var temp = assembly.GetTypes().ToList();
        return assembly.GetTypes().Where(t => trackableInterface.IsAssignableFrom(t)).ToList();
    }

    private static IEnumerable<Type> GetAllModels()
    {
        var assembly = Assembly.GetAssembly(typeof(ApplicationUser));
        var trackableInterface = typeof(ITrackable);
        return assembly.GetTypes().Where(t => trackableInterface.IsAssignableFrom(t)).ToList();
    }

    //        var models = GetAllMarkedServices();
    //            foreach (var model in models)
    //            {
    //                Type serviceInterface = model.GetInterface($"I{model.Name}");
    //                if (serviceInterface != null)
    //                {
    //                    services.AddScoped(serviceInterface, model);
    //                }
    //}
}