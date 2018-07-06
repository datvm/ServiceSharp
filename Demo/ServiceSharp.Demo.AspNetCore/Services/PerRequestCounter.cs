using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSharp.Demo.AspNetCore.Services
{

    [Service]
    public class PerRequestCounter
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
