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
        foreach (var asm in asmList)
        {
            ScanAssembly(asm);
        }

        return services;

        void ScanAssembly(Assembly asm)
        {
            foreach (var t in asm.DefinedTypes)
            {
                if (t.IsAbstract || t.GetCustomAttribute<IgnoreAttribute>() is not null)
                {
                    continue;
                }

                var injectingTypes =
                    GetTypesDeclaredByAttributes(t)
                    .Concat(GetTypesDeclaredByInterfaces(t))
                    .ToHashSet();
            }
        }

        IEnumerable<Type> GetTypesDeclaredByAttributes(Type t)
        {
            var attrs = t.GetCustomAttributes<ServiceAttribute>(false);

            foreach (var attr in attrs)
            {
                if (attr.As is null)
                {
                    foreach (var i in GetInterfaces(t))
                    {
                        yield return i;
                    }
                }
                else
                {
                    yield return attr.As;
                }
            }
        }

        IEnumerable<Type> GetTypesDeclaredByInterfaces(Type t)
        {
            foreach (var i in t.GetInterfaces())
            {
                if (!i.IsAssignableTo(typeof(IService))) { continue; }

                if (i == typeof(IService<>))
                {
                    yield return i.GetGenericArguments()[0];
                }
                else if (i == typeof(IService))
                {
                    foreach (var declInterface in GetInterfaces(t))
                    {
                        yield return declInterface;
                    }
                }
            }
        }

        IEnumerable<Type> GetInterfaces(Type t)
        {
            foreach (var i in t.GetInterfaces())
            {
                if (i.IsAssignableTo(typeof(IService)) ||
                    t.GetCustomAttribute<IgnoreAttribute>() is not null)
                {
                    continue;
                }

                yield return i;
            }
        }

    }

}
