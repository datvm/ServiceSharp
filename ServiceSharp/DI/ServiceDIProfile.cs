using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceSharp.DI
{

    public class ServiceDIProfile
    {

        public HashSet<Type> InjectAsType { get; set; }
        public Type ImplementedType { get; set; }
        public ServiceDILifetime Lifetime { get; set; }

    }

}
