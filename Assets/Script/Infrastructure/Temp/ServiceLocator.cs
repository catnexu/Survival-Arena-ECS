using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public sealed class ServiceLocator : IServiceLocator, IDisposable
    {
        private readonly Dictionary<Type, object> _instanceMap = new();

        public void Dispose()
        {
            foreach ((Type _, var value) in _instanceMap)
            {
                if (value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _instanceMap.Clear();
        }

        public void Register(Type type, object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (!type.IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException($"{instance.GetType().Name} cannot be casted to {type.Name}");
            }

            _instanceMap.Add(type, instance);
        }

        public T Resolve<T>()
        {
            if (_instanceMap.TryGetValue(typeof(T), out var instance))
            {
                return (T) instance;
            }

            throw new KeyNotFoundException($"No instance of type {typeof(T)} was found.");
        }
    }
}