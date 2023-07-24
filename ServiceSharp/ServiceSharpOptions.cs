namespace ServiceSharp;

public class ServiceSharpOptions
{

    public Assembly Assembly { get; set; }
    public List<Assembly> AdditionalAssemblies { get; } = new();

    public ServiceSharpOptions(Assembly assembly)
    {
        Assembly = assembly;
    }

}
