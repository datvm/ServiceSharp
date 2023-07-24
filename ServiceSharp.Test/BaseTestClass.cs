

namespace ServiceSharp.Test;

public class BaseTestClass
{

    public IServiceProvider Setup(Action<IServiceCollection> setupServices)
    {
        var col = new ServiceCollection();
        setupServices(col);

        return col.BuildServiceProvider();
    }

}
