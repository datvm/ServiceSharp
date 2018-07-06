using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ServiceSharp.AspNetCore
{

    public class ServicesOptions
    {

        public List<Assembly> Assemblies { get; set; }

        public ServicesOptions()
        {
            this.Assemblies = new List<Assembly>()
            {
                Assembly.GetEntryAssembly(),
            };
        }

        public static ServicesOptions Build(Action<ServicesOptions> optionBuilder)
        {
            var result = new ServicesOptions();

            optionBuilder?.Invoke(result);

            return result;
        }

    }

}
