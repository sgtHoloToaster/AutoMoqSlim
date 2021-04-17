using Moq;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace AutoMoqSlim
{
    public class AutoMoqer
    {
        readonly ConcurrentDictionary<Type, Mock> _mocks = new();
        readonly IContainer _container;
        readonly MockBehavior _mockBehavior;

        public AutoMoqer(MockBehavior mockBehavior = MockBehavior.Default)
        {
            _mockBehavior = mockBehavior;
            _container = new Container();
        }

        public AutoMoqer(IContainer container, MockBehavior mockBehavior = MockBehavior.Default)
        {
            _mockBehavior = mockBehavior;
            _container = container;
        }

        public object Create(Type type)
        {
            if (_container.TryResolve(type, out var instance) && instance != null)
                return instance;
            else if (_mocks.TryGetValue(type, out var mock))
                return mock.Object;

            var constructor = GetConstructor(type);
            var parameters = GetParameters(constructor);
            return constructor.Invoke(parameters);
        }

        public T Create<T>() =>
            (T)Create(typeof(T));

        private ConstructorInfo GetConstructor(Type type) =>
            type.GetConstructors()
                .Select(c => new { Constructor = c, Parameters = c.GetParameters() })
                .OrderByDescending(cp => cp.Parameters.Length)
                .FirstOrDefault(cp => 
                    cp.Parameters.All(p => p.ParameterType.GetTypeInfo().IsAbstract 
                    || _container.IsRegistered(p.ParameterType)))
                .Constructor;

        private object?[]? GetParameters(ConstructorInfo constructor) =>
            constructor.GetParameters()
                .Select(p => _container.TryResolve(p.ParameterType, out var value) ? value : GetMock(p.ParameterType).Object)
                .ToArray();

        private Mock CreateMock(Type type)
        {
            var constructor = typeof(Mock<>).MakeGenericType(type).GetConstructor(new Type[] { typeof(MockBehavior) });
            return (Mock)constructor.Invoke(new object[] { _mockBehavior });
        }

        public Mock GetMock(Type type) =>
            _mocks.GetOrAdd(type, CreateMock(type));

        public Mock<T> GetMock<T>() where T : class =>
            _mocks.GetOrAdd(typeof(T), new Mock<T>(_mockBehavior)).As<T>();

        public void SetInstance<T>(T instance) =>
            _container.Register(typeof(T), instance);
    }
}
