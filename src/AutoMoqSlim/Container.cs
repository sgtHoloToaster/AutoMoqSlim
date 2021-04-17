using System;
using System.Collections.Generic;

namespace AutoMoqSlim
{
    public class Container
    {
        readonly Dictionary<Type, object?> _registeredInstances = new();

        public void Register<T>(T instance) =>
            Register(typeof(T), instance);

        public void Register(Type type, object? instance) 
        {
            _registeredInstances[type] = instance;
        }

        public bool TryResolve(Type type, out object? instance) =>
            _registeredInstances.TryGetValue(type, out instance) && instance != null;

        public T? Resolve<T>() =>
            (T?)Resolve(typeof(T));

        public object? Resolve(Type type)
        {
            if (_registeredInstances.TryGetValue(type, out var instance))
                return instance;

            return null;
        }

        public bool IsRegistered(Type type) =>
            _registeredInstances.ContainsKey(type);
    }
}
