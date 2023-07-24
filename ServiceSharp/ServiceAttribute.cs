namespace ServiceSharp;


[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ServiceAttribute : Attribute
{

    public Type? ServiceType { get; set; }
    public ServiceLifetime? Lifetime { get; set; }

    public ServiceAttribute() { }

    public ServiceAttribute(Type injectAs)
    {
        ServiceType = injectAs;
    }

}
