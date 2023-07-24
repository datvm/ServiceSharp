# ServiceSharp

ServiceSharp v2.0.0 is released with .NET 6 supports and rewritten from scratch! You no longer need the ASP.NET Core specific package as well.

ServiceSharp is a tiny utility for registering services in .NET Dependency Injection (DI) using **`[Attribute]`** or `Interface`:

## Declare using Attributes

```cs
[Service(typeof(ITestAttr1))] // Explicit declaration
class ImplAttr1 : DefaultImplementation, ITestAttr1 { }

[Ignore]
interface IShouldIgnore {} // Ignore this interface unless explicitly declared

[Service] // Automatically register all implemented interfaces
class ImplAttr23 : 
    ITestAttr2, ITestAttr3,
    IShouldIgnore // Ignored unless explicitly registered
{ }

[Service(typeof(ITestAttr4)), // Multiple explicit declarations
 Service(typeof(ITestAttr5))] // Multiple explicit declarations
class ImplAttr456 : DefaultImplementation, ITestAttr4, ITestAttr5, ITestAttr6 { }

[Service(Lifetime = ServiceLifetime.Scoped)] // Lifetime can be specified
class ImplScoped : ITestAttrScoped { }

[Service(Lifetime = ServiceLifetime.Transient)]
class ImplTransient : ITestAttrTransient { }

[Service(Lifetime = ServiceLifetime.Singleton)]
class ImplSingleton : ITestAttrSingleton { }
```

## Declare using Interfaces

```cs
class ImplInterface1 :
    ITestInterface1,
    ITestInterface2,
    ITestInterface3,           // This service is not registered
    IService<ITestInterface1>, // Explicit declaration
    IService<ITestInterface2>  // Explicit declaration
{ }

[Ignore]
interface IShouldIgnore {} // Ignore this interface unless explicitly declared

// Use Interface declaration for all implemented interfaces
class ImplInterface23 : 
    IService, // Automatically register all implemented interfaces
    ITestInterface2,
    ITestInterface3,
    IShouldIgnore // Ignored unless explicitly registered
{ }

[Ignore] // Do not register this class though it implements IService interface
class ImplInterface3 : 
    ITestInterface3
    IService
{ }
```

> **Note**  
> The lifetime of the service cannot be specified using interface declaration. It uses the value from options instead. The default value is `Scoped`.

# Installation

You can install from [Nuget Package](https://www.nuget.org/packages/ServiceSharp/):

```ps
Install-Package ServiceSharp
```

> **Note**  
> The old package for [ASP.NET Core](https://www.nuget.org/packages/ServiceSharp.AspNetCore/) is now deprecated. You don't need it anymore.

# Usage

Use `AddServices` to register all the declared services in the Assembly:

```
services.AddServices();
```

You can also optionally specify a few options:

```cs
services.AddServices(options => {
    // Change scanning Assembly,
    // the default is Assembly.GetCallingAssembly() which should be your own code
    options.Assembly = Assembly.GetExecutingAssembly();

    // Add more assemblies to be scanned in case you have multiple assemblies
    options.AdditionalAssemblies.Add(GetMyAssembly());

    // Set the default Lifetime for the services if not specified by Attribute declaration
    // The default value is Scoped
    options.Lifetime = ServiceLifetime.Scoped;

    // You can also make and specify your own scanner
    options.TypeScanner = new MyTypeScanner();
});
```