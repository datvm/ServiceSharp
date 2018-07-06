using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceSharp.AspNetCore
{

    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceLifetimeAttribute : Attribute
    {

        public ServiceLifetime Lifetime { get; set; }

        public ServiceLifetimeAttribute(ServiceLifetime lifeTime)
        {
            this.Lifetime = lifeTime;
        }

    }

    public enum ServiceLifetime
    {
        Singleton,
        Scoped,
        Instance,
    }

}
