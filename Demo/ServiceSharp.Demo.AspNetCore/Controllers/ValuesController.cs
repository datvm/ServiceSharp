using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceSharp.Demo.AspNetCore.Filters;
using ServiceSharp.Demo.AspNetCore.Services;

namespace ServiceSharp.Demo.AspNetCore.Controllers
{

    [ApiController]
    public class ValuesController : ControllerBase
    {

        FreshCounter freshCounter;
        PerRequestCounter perRequestCounter;
        LifetimeCounter lifetimeCounter;
        ITimeService timeService;

        public ValuesController(FreshCounter freshCounter, PerRequestCounter perRequestCounter, LifetimeCounter lifetimeCounter, ITimeService timeService)
        {
            this.freshCounter = freshCounter;
            this.perRequestCounter = perRequestCounter;
            this.lifetimeCounter = lifetimeCounter;
            this.timeService = timeService;
        }

        [TypeFilter(typeof(CounterAttribute))]
        [Route("test")]
        public async Task<object> Test()
        {
            return new
            {
                Fresh = await this.freshCounter.Increase(),
                PerRequest = await this.perRequestCounter.Increase(),
                Lifetime = await this.lifetimeCounter.Increase(),
                Now = this.timeService.GetCurrentTime(),
            };
        }

    }
}
