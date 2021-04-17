using System;
using System.Collections.Generic;

namespace AutoMoqSlim
{
    public class Container : IContainer
    {
        readonly Dictionary<Type, object?> _registeredInstances = new();

        public void Register(Type type, object? instance)
        {
            _registeredInstances[type] = instance;
        }

        public bool TryResolve(Type type, out object? instance) =>
            _registeredInstances.TryGetValue(type, out instance) && instance != null;

        public bool IsRegistered(Type type) =>
            _registeredInstances.ContainsKey(type);
    }
}
