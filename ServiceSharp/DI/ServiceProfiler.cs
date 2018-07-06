using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ServiceSharp.DI
{

    public class ServiceProfiler
    {
        public const ServiceDILifetime DefaultServiceLifetime = ServiceDILifetime.Scoped;

        private static readonly Type ignoreType = typeof(IgnoreAttribute);
        private static readonly Type serviceAttributeType = typeof(ServiceAttribute);
        private static readonly Type serviceInterface = typeof(IService);
        private static readonly Type serviceGenericInterface = typeof(IService<>);
        private static readonly Type serviceLifetimeAttribute = typeof(ServiceLifetimeAttribute);

        public HashSet<Assembly> Assemblies { get; private set; } = new HashSet<Assembly>();
        public List<ServiceDIProfile> Services { get; private set; } = new List<ServiceDIProfile>();

        public ServiceProfiler() { }

        public void Scan(Assembly assembly)
        {
            this.Assemblies.Add(assembly);

            this.Scan(assembly, assembly.GetTypes());
        }

        public void Scan(Assembly assembly, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                if (IsService(type, out var customAttributes, out var implementedInterfaces))
                {
                    var injectAs = GetInjectAsType(type, customAttributes, implementedInterfaces);
                    var lifetime = GetServiceLifetime(type, customAttributes);

                    this.Services.Add(new ServiceDIProfile()
                    {
                        InjectAsType = injectAs,
                        ImplementedType = type,
                        Lifetime = lifetime,
                    });
                }
            }
        }

        internal static HashSet<Type> GetInjectAsType(Type type, object[] customAttributes, Type[] implementedInterfaces)
        {
            var result = new HashSet<Type>();

            // Get from Attribute
            var serviceAttributes = customAttributes
                .Where(q => q is ServiceAttribute)
                .Cast<ServiceAttribute>();

            foreach (var serviceAttribute in serviceAttributes)
            {
                result.Add(serviceAttribute.As ?? type);
            }

            // Get from Interface
            foreach (var implInterface in implementedInterfaces)
            {
                if (implInterface.IsGenericType && 
                    implInterface.GetGenericTypeDefinition() == serviceGenericInterface)
                {
                    var genericType = implInterface.GetGenericArguments()[0];
                    result.Add(genericType);
                }
            }

            // Add self if empty
            if (result.Count == 0)
            {
                result.Add(type);
            }
            
            return result;
        }

        internal static ServiceDILifetime GetServiceLifetime(Type type, object[] customAttributes)
        {
            var result = DefaultServiceLifetime;

            var serviceLifetimeAttribute = customAttributes
                .FirstOrDefault(q => q is ServiceLifetimeAttribute);
            if (serviceLifetimeAttribute != null)
            {
                result = (serviceLifetimeAttribute as ServiceLifetimeAttribute).Lifetime;
            }

            return result;
        }

        internal static bool IsService(Type type, out object[] customAttributes, out Type[] implementedInterfaces)
        {
            customAttributes = null;
            implementedInterfaces = null;

            // Must be a concrete Class
            if (!type.IsClass || type.IsAbstract)
            {
                return false;
            }

            customAttributes = type.GetCustomAttributes(true);
            implementedInterfaces = type.GetInterfaces();

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

            if (implementedInterfaces.Any(q => q == serviceInterface))
            {
                return true;
            }

            return false;
        }

    }

}
