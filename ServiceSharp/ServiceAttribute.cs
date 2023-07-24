namespace ServiceSharp;


[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ServiceAttribute : Attribute
{

    public Type? As { get; set; }

    public ServiceAttribute() { }

    public ServiceAttribute(Type injectAs)
    {
        As = injectAs;
    }

}
