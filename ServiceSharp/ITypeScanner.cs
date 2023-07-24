namespace ServiceSharp;

public interface ITypeScanner
{

    public IEnumerable<ServiceDescriptor> ScanForTypes(IEnumerable<Assembly> assemblies, ServiceSharpOptions options);

}