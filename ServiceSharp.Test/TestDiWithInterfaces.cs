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
    public void ShouldGetExplicitOnly()
    {
        var services = Setup();

        var impl4 = services.GetRequiredService<ITestInterface4>();
        Assert.IsType<ImplInterface456>(impl4);

        var impl5 = services.GetRequiredService<ITestInterface5>();
        Assert.IsType<ImplInterface456>(impl5);

        Assert.Throws<InvalidOperationException>(() =>
        {
            services.GetRequiredService<ITestInterface6>();
        });
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
interface ITestInterface4 { }
interface ITestInterface5 { }
interface ITestInterface6 { }

class ImplInterface1 : DefaultImplementation, ITestInterface1, IService<ITestInterface1>
{

}

class ImplInterface23 : DefaultImplementation, IService, ITestInterface2, ITestInterface3, IShouldIgnore
{

}

class ImplInterface456 : DefaultImplementation,
    ITestInterface4,
    ITestInterface5,
    ITestInterface6,
    IService<ITestInterface4>,
    IService<ITestInterface5>
{

}