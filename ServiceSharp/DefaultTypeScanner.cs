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

    IEnumerable<ServiceDescriptor> GetTypesDeclaredByAttributes(Type t, ServiceSharpOptions options)
    {
        var attrs = t.GetCustomAttributes<ServiceAttribute>(false);

        foreach (var attr in attrs)
        {
            var lifetime = attr.Lifetime ?? options.DefaultLifetime;
            if (attr.ServiceType is null)
            {                
                foreach (var i in GetInterfaces(t))
                {
                    yield return new(i, t, lifetime);
                }
            }
            else
            {
                yield return new(attr.ServiceType, t, lifetime);
            }
        }
    }

    IEnumerable<ServiceDescriptor> GetTypesDeclaredByInterfaces(Type t, ServiceSharpOptions options)
    {
        foreach (var i in t.GetInterfaces())
        {
            if (!i.IsAssignableTo(typeof(IService))) { continue; }

            if (i == typeof(IService<>))
            {
                yield return new(i.GetGenericArguments()[0], t, options.DefaultLifetime);
            }
            else if (i == typeof(IService))
            {
                foreach (var declInterface in GetInterfaces(t))
                {
                    yield return new(declInterface, t, options.DefaultLifetime);
                }
            }
        }
    }

    static IEnumerable<Type> GetInterfaces(Type t)
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
