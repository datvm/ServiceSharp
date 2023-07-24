

namespace ServiceSharp.Test;

public class BaseTestClass
{

    public IServiceProvider Setup() =>
        Setup(null);

    public IServiceProvider Setup(ServiceLifetime? defaultLifetime)
    {
        var col = new ServiceCollection();

        if (defaultLifetime.HasValue)
        {
            col.AddServices(options =>
            {
                options.DefaultLifetime = defaultLifetime.Value;
            });
        }
        else
        {
            col.AddServices();
        }

        return col.BuildServiceProvider();
    }

}
