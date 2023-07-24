namespace ServiceSharp.Test;

public interface ITestInterface1
{
    int Increase();
}

public interface ITestInterface2
{
    int Decrease();
}

[Ignore]
public interface IShouldIgnore
{}

public class DefaultImpl1 : ITestInterface1, IShouldIgnore
{
    int curr = 0;

    public int Increase()
    {
        return ++curr;
    }
}

public class DefaultImpl2 : ITestInterface2, IShouldIgnore
{
    int curr = 0;

    public int Decrease()
    {
        return --curr;
    }
}

public class DefaultImpl12 : ITestInterface1, ITestInterface2, IShouldIgnore
{
    int curr = 0;

    public int Decrease()
    {
        return --curr;
    }

    public int Increase()
    {
        return ++curr;
    }
}