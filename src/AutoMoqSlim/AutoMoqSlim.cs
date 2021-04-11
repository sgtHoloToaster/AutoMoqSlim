using Moq;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace AutoMoqSlim
{
    public class AutoMoqSlim
    {
        readonly ConcurrentDictionary<Type, Mock> _mocks = new();

        public T Create<T>()
        {
            var constructor = typeof(T).GetConstructors().FirstOrDefault();
            var parameters = constructor.GetParameters()
                .Select(p => GetMock(p.ParameterType).Object)
                .ToArray();

            return (T)constructor.Invoke(parameters);
        }

        private Mock CreateMock(Type type)
        {
            var constructor = typeof(Mock<>).MakeGenericType(type).GetConstructor(new Type[] { });
            return (Mock)constructor.Invoke(new object[] { });
        }

        private Mock GetMock(Type type) =>
            _mocks.GetOrAdd(type, CreateMock(type));

        public Mock<T> GetMock<T>() where T : class =>
            _mocks.GetOrAdd(typeof(T), new Mock<T>()).As<T>();

        public void SetInstance<T>(T instance)
        {
            throw new NotImplementedException();
        }
    }
}
