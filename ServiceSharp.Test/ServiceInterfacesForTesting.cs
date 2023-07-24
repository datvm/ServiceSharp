namespace ServiceSharp.Test;

public class DefaultImplementation
{
    int counter = 0;

    public int Increase() => ++counter;
    public int Decrease() => --counter;

}

[Ignore]
public interface IShouldIgnore { }