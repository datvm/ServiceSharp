using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceSharp.AspNetCore
{

    public static class ServiceExtensions
    {
        public const ServiceLifetime DefaultServiceLifetime = ServiceLifetime.Scoped;

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return AddServices(services, ServicesOptions.Build(null));
        }

        public static IServiceCollection AddServices(this IServiceCollection services, ServicesOptions options)
        {
            options = options ?? new ServicesOptions();

            var ignoreType = typeof(IgnoreAttribute);
            var serviceInterface = typeof(IService);
            var serviceLifetimeAttribute = typeof(ServiceLifetimeAttribute);

            foreach (var assembly in options.Assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && serviceInterface.IsAssignableFrom(type))
                    {
                        var lifetime = DefaultServiceLifetime;

                        var attribute = type.GetCustomAttributes(serviceLifetimeAttribute, true)
                            .FirstOrDefault();
                        if (attribute != null)
                        {
                            lifetime = (attribute as ServiceLifetimeAttribute).Lifetime;
                        }

                        switch (lifetime)
                        {
                            case ServiceLifetime.Singleton:
                                services.AddSingleton(type);
                                break;
                            case ServiceLifetime.Scoped:
                                services.AddScoped(type);
                                break;
                            case ServiceLifetime.Instance:
                                services.AddTransient(type);
                                break;
                            default:
                                throw new ArgumentException("Unknown service lifetime: " + lifetime);
                        }
                    }
                }
            }

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, Action<ServicesOptions> optionsBuilder)
        {
            return AddServices(services, ServicesOptions.Build(optionsBuilder));
        }

    }

}
