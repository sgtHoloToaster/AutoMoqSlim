using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoMoqSlim
{
    public class AutoMoqSlim
    {
        readonly ConcurrentDictionary<Type, Mock> _mocks = new();
        readonly Dictionary<Type, object?> _registeredInstances = new();

        public T Create<T>()
        {
            var constructor = GetConstructor<T>();
            var parameters = constructor.GetParameters()
                .Select(p => _registeredInstances.TryGetValue(p.ParameterType, out var value) ? value : GetMock(p.ParameterType).Object)
                .ToArray();

            return (T)constructor.Invoke(parameters);
        }

        private ConstructorInfo GetConstructor<T>() =>
            typeof(T).GetConstructors()
                .Select(c => (Constructor: c, Parameters: c.GetParameters()))
                .OrderByDescending(cp => cp.Parameters.Length)
                .FirstOrDefault(cp => cp.Parameters.All(p => p.ParameterType.IsAbstract || _registeredInstances.ContainsKey(p.ParameterType)))
                .Constructor;

        private Mock CreateMock(Type type)
        {
            var constructor = typeof(Mock<>).MakeGenericType(type).GetConstructor(new Type[] { });
            return (Mock)constructor.Invoke(new object[] { });
        }

        private Mock GetMock(Type type) =>
            _mocks.GetOrAdd(type, CreateMock(type));

        public Mock<T> GetMock<T>() where T : class =>
            _mocks.GetOrAdd(typeof(T), new Mock<T>()).As<T>();

        public void SetInstance<T>(T instance) =>
            _registeredInstances[typeof(T)] = instance;
    }
}
