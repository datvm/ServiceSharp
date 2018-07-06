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

        private static readonly Type ignoreType = typeof(IgnoreAttribute);
        private static readonly Type serviceAttributeType = typeof(ServiceAttribute);
        private static readonly Type serviceInterface = typeof(IService);
        private static readonly Type serviceLifetimeAttribute = typeof(ServiceLifetimeAttribute);

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return AddServices(services, ServicesOptions.Build(null));
        }

        public static IServiceCollection AddServices(this IServiceCollection services, ServicesOptions options)
        {
            options = options ?? new ServicesOptions();

            foreach (var assembly in options.Assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsService())
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

        private static bool IsService(this Type type)
        {
            // Must be a concrete Class
            if (!type.IsClass && !type.IsAbstract)
            {
                return false;
            }

            var customAttributes = type.GetCustomAttributes(true);

            // No IgnoreAttribute
            if (customAttributes.Any(q => q is IgnoreAttribute))
            {
                return false;
            }

            // Have either ServiceAttribute or implement IService
            if (customAttributes.Any(q => q is ServiceAttribute))
            {
                return true;
            }

            if (serviceInterface.IsAssignableFrom(type))
            {
                return true;
            }

            return false;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, Action<ServicesOptions> optionsBuilder)
        {
            return AddServices(services, ServicesOptions.Build(optionsBuilder));
        }

    }

}
