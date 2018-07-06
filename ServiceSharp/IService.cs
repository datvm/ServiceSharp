using System;

namespace ServiceSharp
{

    public interface IService { }

    public interface IService<TInterface> : IService { }

}
