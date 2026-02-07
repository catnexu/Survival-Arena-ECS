using System;

namespace Infrastructure
{
    public interface IServiceLocator
    {
        T Resolve<T>();
        void Register(Type type, object instance);
    }
}