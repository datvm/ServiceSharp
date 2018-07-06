using ServiceSharp.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSharp.Demo.AspNetCore.Services
{

    [ServiceLifetime(ServiceDILifetime.Instance)]
    public class FreshCounter : IService
    {

        private int counter;

        public async Task<int> Increase()
        {
            return await Task.Run(() =>
            {
                this.counter++;
                return this.counter;
            });
        }

    }

}
