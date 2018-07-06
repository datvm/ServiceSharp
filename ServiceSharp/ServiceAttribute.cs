using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceSharp
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ServiceAttribute : Attribute
    {

        public Type As { get; set; }

        public ServiceAttribute() { }

        public ServiceAttribute(Type injectAs)
        {
            this.As = injectAs;
        }

    }

}
