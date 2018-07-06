using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSharp.Demo.AspNetCore.Services
{

    public interface ITimeService : IService
    {
        DateTime GetCurrentTime();
    }

    public class TimeService : ITimeService, IService<ITimeService>
    {

        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

    }

}
