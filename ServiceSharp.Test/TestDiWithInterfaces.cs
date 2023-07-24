namespace ServiceSharp.Test;
public class TestDiWithInterfaces : BaseTestClass
{

    [Fact]
    public void ShouldGetImplFromInterfaces()
    {
        var services = Setup();

        var impl1 = services.GetRequiredService<ITestInterface1>();
        Assert.IsType<ImplInterface1>(impl1);

        var impl2 = services.GetRequiredService<ITestInterface2>();
        Assert.IsType<ImplInterface23>(impl2);

        var impl3 = services.GetRequiredService<ITestInterface3>();
        Assert.IsType<ImplInterface23>(impl3);
    }

    [Fact]
    public void ShouldIgnore()
    {
        var services = Setup();

        Assert.Throws<InvalidOperationException>(() =>
        {
            services.GetRequiredService<IShouldIgnore>();
        });
    }

}

interface ITestInterface1 { }
interface ITestInterface2 { }
interface ITestInterface3 { }

class ImplInterface1 : DefaultImplementation, ITestInterface1, IService<ITestInterface1>
{

}

class ImplInterface23 : DefaultImplementation, IService, ITestInterface2, ITestInterface3, IShouldIgnore
{

}