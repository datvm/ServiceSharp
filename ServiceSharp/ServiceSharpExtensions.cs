global using System.Reflection;
global using Microsoft.Extensions.DependencyInjection;

namespace ServiceSharp;

public static class ServiceSharpExtensions
{

    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddServices(null);

    public static IServiceCollection AddServices(
        this IServiceCollection services,
        Action<ServiceSharpOptions>? configure)
    {
        var options = new ServiceSharpOptions(Assembly.GetCallingAssembly());
        configure?.Invoke(options);

        var asmList = options.AdditionalAssemblies.Prepend(options.Assembly);

        var descriptors = options.TypeScanner.ScanForTypes(asmList, options);
        foreach (var desc in descriptors)
        {
            services.Add(desc);
        }

        return services;

    }

}
