namespace ServiceSharp;

public class ServiceSharpOptions
{

    public Assembly Assembly { get; set; }
    public List<Assembly> AdditionalAssemblies { get; } = new();

    public ServiceLifetime DefaultLifetime { get; set; } = ServiceLifetime.Scoped;

    public ITypeScanner TypeScanner { get; set; } = new DefaultTypeScanner();

    public ServiceSharpOptions(Assembly assembly)
    {
        Assembly = assembly;
    }

}
