using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceSharp.DI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceLifetimeAttribute : Attribute
    {

        public ServiceDILifetime Lifetime { get; set; }

        public ServiceLifetimeAttribute(ServiceDILifetime lifeTime)
        {
            this.Lifetime = lifeTime;
        }

    }
}
