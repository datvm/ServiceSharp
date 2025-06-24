namespace ServiceSharp;

public class DefaultDescriptorComparer : IEqualityComparer<ServiceDescriptor>
{
    public bool Equals(ServiceDescriptor? x, ServiceDescriptor? y)
    {
        return x?.ServiceType.Equals(y?.ServiceType) == true;
    }

    public int GetHashCode(ServiceDescriptor obj)
    {
        return obj.ServiceType.GetHashCode();
    }
}

public class DefaultTypeScanner : ITypeScanner
{

    public virtual IEnumerable<ServiceDescriptor> ScanForTypes(IEnumerable<Assembly> assemblies, ServiceSharpOptions options)
    {
        var descriptors = new HashSet<ServiceDescriptor>(new DefaultDescriptorComparer());

        foreach (var asm in assemblies)
        {
            foreach (var desc in ScanAssembly(asm, options))
            {
                descriptors.Add(desc);
            }
        }

        return descriptors;
    }

    public virtual IEnumerable<ServiceDescriptor> ScanAssembly(Assembly asm, ServiceSharpOptions options)
    {
        foreach (var t in asm.DefinedTypes)
        {
            if (t.IsAbstract || t.GetCustomAttribute<IgnoreAttribute>() is not null)
            {
                continue;
            }

            foreach (var item in GetTypesDeclaredByAttributes(t, options))
            {
                yield return item;
            }

            foreach (var item in GetTypesDeclaredByInterfaces(t, options))
            {
                yield return item;
            }
        }
    }

    static IEnumerable<ServiceDescriptor> GetTypesDeclaredByAttributes(Type t, ServiceSharpOptions options)
    {
        var attrs = t.GetCustomAttributes<ServiceAttribute>(false);

        foreach (var attr in attrs)
        {
            var lifetime = attr.ActualLifetime ?? options.DefaultLifetime;
            if (attr.ServiceType is null)
            {
                if (attr is SelfServiceAttribute)
                {
                    yield return new(t, t, lifetime);
                }
                else
                {
                    foreach (var i in GetInterfaces(t))
                    {
                        yield return new(i, t, lifetime);
                    }
                }
            }
            else
            {
                yield return new(attr.ServiceType, t, lifetime);
            }
        }
    }

    static IEnumerable<ServiceDescriptor> GetTypesDeclaredByInterfaces(Type t, ServiceSharpOptions options)
    {
        var hasService = false;
        var hasGenericService = false;

        foreach (var i in t.GetInterfaces())
        {
            if (!typeof(IService).IsAssignableFrom(i)) { continue; }

            if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IService<>))
            {
                hasGenericService = true;
                yield return new(i.GetGenericArguments()[0], t, options.DefaultLifetime);
            }
            else if (i == typeof(IService))
            {
                hasService = true;
            }
        }

        if (!hasGenericService && hasService)
        {
            foreach (var declInterface in GetInterfaces(t))
            {
                yield return new(declInterface, t, options.DefaultLifetime);
            }
        }
    }

    static IEnumerable<Type> GetInterfaces(Type t)
    {
        foreach (var i in t.GetInterfaces())
        {
            if (typeof(IService).IsAssignableFrom(i) ||
                i.GetCustomAttribute<IgnoreAttribute>() is not null)
            {
                continue;
            }

            yield return i;
        }
    }

}
