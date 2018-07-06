using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSharp.Demo.AspNetCore.Services
{

    public class TimeService : IService
    {

        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

    }

}
