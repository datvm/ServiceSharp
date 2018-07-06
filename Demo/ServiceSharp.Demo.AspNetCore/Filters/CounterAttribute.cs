using Microsoft.AspNetCore.Mvc.Filters;
using ServiceSharp.Demo.AspNetCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSharp.Demo.AspNetCore.Filters
{

    public class CounterAttribute : ActionFilterAttribute
    {

        FreshCounter freshCounter;
        PerRequestCounter perRequestCounter;
        LifetimeCounter lifetimeCounter;

        public CounterAttribute(FreshCounter freshCounter, PerRequestCounter perRequestCounter, LifetimeCounter lifetimeCounter)
        {
            this.freshCounter = freshCounter;
            this.perRequestCounter = perRequestCounter;
            this.lifetimeCounter = lifetimeCounter;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Response.Headers.Add("Fresh-Counter-1", (await this.freshCounter.Increase()).ToString());
            context.HttpContext.Response.Headers.Add("Fresh-Counter-2", (await this.freshCounter.Increase()).ToString());

            context.HttpContext.Response.Headers.Add("Per-Request-Counter-1", (await this.perRequestCounter.Increase()).ToString());
            context.HttpContext.Response.Headers.Add("Per-Request-Counter-2", (await this.perRequestCounter.Increase()).ToString());

            context.HttpContext.Response.Headers.Add("Lifetime-Counter-1", (await this.lifetimeCounter.Increase()).ToString());
            context.HttpContext.Response.Headers.Add("Lifetime-Counter-2", (await this.lifetimeCounter.Increase()).ToString());

            await base.OnActionExecutionAsync(context, next);

            context.HttpContext.Response.Headers.Add("Fresh-Counter-3", (await this.freshCounter.Increase()).ToString());
            context.HttpContext.Response.Headers.Add("Fresh-Counter-4", (await this.freshCounter.Increase()).ToString());

            context.HttpContext.Response.Headers.Add("Per-Request-Counter-3", (await this.perRequestCounter.Increase()).ToString());
            context.HttpContext.Response.Headers.Add("Per-Request-Counter-4", (await this.perRequestCounter.Increase()).ToString());

            context.HttpContext.Response.Headers.Add("Lifetime-Counter-3", (await this.lifetimeCounter.Increase()).ToString());
            context.HttpContext.Response.Headers.Add("Lifetime-Counter-4", (await this.lifetimeCounter.Increase()).ToString());
        }

    }

}
