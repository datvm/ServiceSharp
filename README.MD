# ServiceSharp

ServiceSharp is a tiny utility for applying Repository-Service pattern in C#, with sub-packages for ASP.NET Core (ASP.NET MVC is in plan, but since there is no concrete implementation on any DI framework, it will be framework-dependent).

You can install from [Nuget Package](https://www.nuget.org/packages/ServiceSharp/):

`> Install-Package ServiceSharp`

There is also a package for [ASP.NET Core](https://www.nuget.org/packages/ServiceSharp.AspNetCore/):

`> Install-Package ServiceSharp.AspNetCore`

# Advantages

ServiceSharp enables Attribute decorations or Interface for Services throughout your project. You can [check the full demo here](https://github.com/datvm/ServiceSharp/tree/master/Demo/).

# Usage with Dependency Injection (DI)

After installing the packages, you can decorate all your services with either the Interface `ServiceSharp.IService` or the Attribute `ServiceAttribute`.

```
    public class TimeService : IService
    {

        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

    }

	[Service]
    public class PerRequestCounter
    {
		// Other code
	}
```

In your `Configure` method, you can call the `AddServices()` extension method (from `ServiceSharp.AspNetCore` namespace in case Visual Studio does not suggest it for you):

```
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddServices();
        }
```

# Service Scope/Lifetime

If you want your `IService` implemented classes to be ignored by ServiceSharp, simply decorate it with the `IgnoreAttribute` (from `ServiceSharp` namespace):

```
    [Ignore]
    public class TimeService : IService
```

If you do not specify the Service Scope/Lifetime, ServiceSharp will use the default one stated in `ServiceExtensions.DefaultServiceLifetime` (Scoped as in current version). Other `ServiceLifetime` enum values include `Singleton` and `Instance`:

```
    [ServiceLifetime(ServiceLifetime.Instance)]
    public class FreshCounter : IService
    {
	}

	[ServiceLifetime(ServiceLifetime.Singleton)]
    public class LifetimeCounter : IService
    {
	}

	// No decoration because it is the default one.
	// You can still decorate it with ServiceLifetime.Scoped value.
	[Service]
    public class PerRequestCounter
    {
	}
```

# Additional Assemblies

Without any options specified, `GetEntryAssembly()` is used to search for classes that implement `IService`. If you want to inject `Service`s in your other assemblies (libraries) too, you can specify it in the options:

```
    services.AddServices(options =>
    {
        options.Assemblies = new List<System.Reflection.Assembly>()
        {
            typeof(Startup).Assembly, // If you override it, do not forget to add this back
            typeof(MyLibrary).Assembly,
        },
    });
```

# Where is Repository?

For this version there is no `Repository` or `UnitOfWork` yet because I am using Entity Framework, which by itself is a good Repository and Unit of Work design already. As for now, you can already mark them with the same IService interface and DI will inject them into Services just fine.