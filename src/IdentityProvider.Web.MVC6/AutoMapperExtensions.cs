using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace IdentityProvider.Web.MVC6
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddApplicationAutoMapper(this IServiceCollection services)
        {
            // Assuming "IdentityProvider.ServiceLayer" is the correct namespace where your mapping profiles are located.
            // Adjust the assembly scanning as necessary to accurately locate your profiles.
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => assembly.FullName.StartsWith("IdentityProvider.ServiceLayer")));
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
