namespace ServiceSharp.Test;

public class TestDiWithAttributes : BaseTestClass
{


    [Fact]
    public void ShouldGetImplFromAttributes()
    {
        var services = Setup();

        var impl1 = services.GetRequiredService<ITestAttr1>();
        Assert.IsType<ImplAttr1>(impl1);

        var impl2 = services.GetRequiredService<ITestAttr2>();
        Assert.IsType<ImplAttr23>(impl2);

        var impl3 = services.GetRequiredService<ITestAttr3>();
        Assert.IsType<ImplAttr23>(impl3);
    }

    [Fact]
    public void ShouldIgnore()
    {
        var services = Setup();

        Assert.Throws<InvalidOperationException>(() =>
        {
            return services.GetRequiredService<IShouldIgnore>();
        });
    }

    [Fact]
    public void ShouldGetSingleton()
    {
        var services = Setup();

        ITestAttrSingleton attr1, attr2;
        using (var scope = services.CreateScope())
        {
            attr1 = scope.ServiceProvider.GetRequiredService<ITestAttrSingleton>();
        }

        using (var scope = services.CreateScope())
        {
            attr2 = scope.ServiceProvider.GetRequiredService<ITestAttrSingleton>();
        }

        Assert.Same(attr1, attr2);
        Assert.Equal(1, attr1.Increase());
        Assert.Equal(2, attr2.Increase());
    }

    [Fact]
    public void ShouldGetScoped()
    {
        var services = Setup();

        ITestAttrScoped attr1, attr2, attr3;
        using (var scope = services.CreateScope())
        {
            attr1 = scope.ServiceProvider.GetRequiredService<ITestAttrScoped>();
            attr2 = scope.ServiceProvider.GetRequiredService<ITestAttrScoped>();
        }

        using (var scope = services.CreateScope())
        {
            attr3 = scope.ServiceProvider.GetRequiredService<ITestAttrScoped>();
        }

        Assert.Same(attr1, attr2);
        Assert.NotSame(attr1, attr3);

        Assert.Equal(1, attr1.Increase());
        Assert.Equal(2, attr2.Increase());
        Assert.Equal(1, attr3.Increase());
    }

    [Fact]
    public void ShouldGetTransient()
    {
        var services = Setup();

        ITestAttrTransient attr1, attr2, attr3;

        attr3 = services.GetRequiredService<ITestAttrTransient>();
        using (var scope = services.CreateScope())
        {
            attr1 = scope.ServiceProvider.GetRequiredService<ITestAttrTransient>();
            attr2 = scope.ServiceProvider.GetRequiredService<ITestAttrTransient>();
        }

        Assert.NotSame(attr1, attr2);
        Assert.NotSame(attr1, attr3);
        Assert.Equal(1, attr1.Increase());
        Assert.Equal(1, attr2.Increase());
        Assert.Equal(1, attr3.Increase());
    }

    [Fact]
    public void ShouldGetSingletonDefault()
    {
        var services = Setup(ServiceLifetime.Singleton);

        ITestAttr1 testAttr1, testAttr2;
        using (var scope = services.CreateScope())
        {
            testAttr1 = scope.ServiceProvider.GetRequiredService<ITestAttr1>();
        }

        using (var scope = services.CreateScope())
        {
            testAttr2 = scope.ServiceProvider.GetRequiredService<ITestAttr1>();
        }

        Assert.Same(testAttr1, testAttr2);
    }

    [Fact]
    public void ShouldGetScopedDefault()
    {
        var services = Setup();

        ITestAttr1 attr1, attr2, attr3;

        using (var scope = services.CreateScope())
        {
            attr1 = scope.ServiceProvider.GetRequiredService<ITestAttr1>();
            attr2 = scope.ServiceProvider.GetRequiredService<ITestAttr1>();
        }

        using (var scope = services.CreateScope())
        {
            attr3 = scope.ServiceProvider.GetRequiredService<ITestAttr1>();
        }

        Assert.Same(attr1, attr2);
        Assert.NotSame(attr1, attr3);
    }

    [Fact]
    public void ShouldGetTransientDefault()
    {
        var services = Setup(ServiceLifetime.Transient);

        ITestAttr1 attr1, attr2, attr3;

        attr3 = services.GetRequiredService<ITestAttr1>();
        using (var scope = services.CreateScope())
        {
            attr1 = scope.ServiceProvider.GetRequiredService<ITestAttr1>();
            attr2 = scope.ServiceProvider.GetRequiredService<ITestAttr1>();
        }

        Assert.NotSame(attr1, attr2);
        Assert.NotSame(attr1, attr3);
    }
}


interface ITestAttr1 { }
interface ITestAttr2 { }
interface ITestAttr3 { }
interface ITestAttrSingleton { int Increase(); }
interface ITestAttrScoped { int Increase(); }
interface ITestAttrTransient { int Increase(); }

[Service(typeof(ITestAttr1))]
class ImplAttr1 : DefaultImplementation, ITestAttr1 { }

[Service]
class ImplAttr23 : DefaultImplementation, IShouldIgnore, ITestAttr2, ITestAttr3 { }

[Service(Lifetime = ServiceLifetime.Scoped)]
class ImplScoped : DefaultImplementation, ITestAttrScoped { }

[Service(Lifetime = ServiceLifetime.Transient)]
class ImplTransient : DefaultImplementation, ITestAttrTransient { }

[Service(Lifetime = ServiceLifetime.Singleton)]
class ImplSingleton : DefaultImplementation, ITestAttrSingleton { }