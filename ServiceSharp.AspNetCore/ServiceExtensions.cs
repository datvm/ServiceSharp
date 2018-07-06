using Microsoft.Extensions.DependencyInjection;
using ServiceSharp.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceSharp.AspNetCore
{

    public static class ServiceExtensions
    {
        
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return AddServices(services, ServicesOptions.Build(null));
        }

        public static IServiceCollection AddServices(this IServiceCollection services, Action<ServicesOptions> optionsBuilder)
        {
            return AddServices(services, ServicesOptions.Build(optionsBuilder));
        }

        public static IServiceCollection AddServices(this IServiceCollection services, ServicesOptions options)
        {
            options = options ?? new ServicesOptions();

            var profiler = new ServiceProfiler();
            foreach (var assembly in options.Assemblies)
            {
                profiler.Scan(assembly);
            }

            foreach (var service in profiler.Services)
            {
                Func<Type, Type, IServiceCollection> registerAction;

                switch (service.Lifetime)
                {
                    case DI.ServiceDILifetime.Singleton:
                        registerAction = services.AddSingleton;
                        break;
                    case DI.ServiceDILifetime.Scoped:
                        registerAction = services.AddScoped;
                        break;
                    case DI.ServiceDILifetime.Instance:
                        registerAction = services.AddTransient;
                        break;
                    default:
                        throw new ArgumentException("Unknown service lifetime: " + service.Lifetime);
                }

                foreach (var injectedAs in service.InjectAsType)
                {
                    registerAction(injectedAs, service.ImplementedType);
                }
            }

            return services;
        }

    }

}
