using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public sealed class ServiceLocator : IServiceLocator, IDisposable
    {
        private readonly Dictionary<Type, object> _instanceMap = new();

        public void Dispose()
        {
            foreach ((Type key, object value) in _instanceMap)
            {
                if (value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _instanceMap.Clear();
        }

        public void Register<T>(T instance) where T : class => _instanceMap.Add(typeof(T), instance);

        public T Resolve<T>()
        {
            if (_instanceMap.TryGetValue(typeof(T), out object instance))
            {
                return (T) instance;
            }

            throw new KeyNotFoundException($"No instance of type {typeof(T)} was found.");
        }
    }
}