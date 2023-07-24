namespace ServiceSharp.Test;
public class TestDiWithInterfaces
{

    

}

class ImplInterface1 : DefaultImpl1, IService
{

}

class ImplInterface2 : DefaultImpl1, IService<ITestInterface1>
{

}