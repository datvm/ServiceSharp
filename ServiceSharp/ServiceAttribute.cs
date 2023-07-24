namespace ServiceSharp;


[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ServiceAttribute : Attribute
{
    public Type? ServiceType { get; set; }

    bool hasSetLifetime = false;
    ServiceLifetime lifetime;
    public ServiceLifetime Lifetime
    {
        get => lifetime;
        set
        {
            lifetime = value;
            hasSetLifetime = true;
        }
    }

    public ServiceLifetime? ActualLifetime => hasSetLifetime ? 
        lifetime : 
        null;

    public ServiceAttribute() { }

    public ServiceAttribute(Type injectAs)
    {
        ServiceType = injectAs;
    }

}