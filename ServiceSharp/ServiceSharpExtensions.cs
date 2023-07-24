global using System.Reflection;
global using Microsoft.Extensions.DependencyInjection;
global using ServiceSharp;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceSharpExtensions
{

    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddServices(Assembly.GetCallingAssembly(), null);

    public static IServiceCollection AddServices(
        this IServiceCollection services,
        Action<ServiceSharpOptions>? configure) =>
        services.AddServices(Assembly.GetCallingAssembly(), configure);

    public static IServiceCollection AddServices(
        this IServiceCollection services,
        Assembly assembly,
        Action<ServiceSharpOptions>? configure)
    {
        var options = new ServiceSharpOptions(assembly);
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
