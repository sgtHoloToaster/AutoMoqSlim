using AutoMoqSlim.Exceptions;
using Moq;
using Moq.Language.Flow;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
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


        /// <summary>
        /// Returns the instance registered in the container or creates a new one of <paramref name="type"/>. 
        /// For new instances, any abstract dependencies, not registered in the container, will be replaced with mocks. 
        /// </summary>
        /// <param name="type">The type to create</param>
        /// <returns>An instance of <paramref name="type" /></returns>
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

        /// <summary>
        /// Creates an instance of <typeparamref name="T"/>. Any abstract dependencies, not registered in the container, will be replaced with mocks
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        /// <returns>An instance of <typeparamref name="T" /></returns>
        public T Create<T>() =>
            (T)Create(typeof(T));

        private ConstructorInfo GetConstructor(Type type)
        {
            var constructors = type.GetConstructors();
            if (constructors.Length == 0)
                throw new NoPublicConstructorException(type);

            return constructors
                .Select(c => new { Constructor = c, Parameters = c.GetParameters() })
                .OrderByDescending(cp => cp.Parameters.Length)
                .FirstOrDefault(cp =>
                    cp.Parameters.All(p => p.ParameterType.GetTypeInfo().IsAbstract
                    || _container.IsRegistered(p.ParameterType)))
                .Constructor;
        }

        private object?[]? GetParameters(ConstructorInfo constructor) =>
            constructor.GetParameters()
                .Select(p => _container.TryResolve(p.ParameterType, out var value) ? value : GetMock(p.ParameterType).Object)
                .ToArray();

        private Mock CreateMock(Type type)
        {
            var constructor = typeof(Mock<>).MakeGenericType(type).GetConstructor(new Type[] { typeof(MockBehavior) });
            return (Mock)constructor.Invoke(new object[] { _mockBehavior });
        }

        /// <summary>
        /// Gets the mock that will be passed as a dependency to an object, created by the Create method, if the object has a 
        /// dependency of type <paramref name="type"/>
        /// </summary>
        /// <param name="type">Type to mock</param>
        /// <returns>Mock for a dependency with type <paramref name="type"/></returns>
        public Mock GetMock(Type type) =>
            _mocks.GetOrAdd(type, CreateMock(type));

        /// <summary>
        /// Gets the mock that will be passed as a dependency to an object, created by the Create method, if the object has a 
        /// dependency of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type to mock</typeparam>
        /// <returns>Mock for a dependency with type <typeparamref name="T"/></returns>
        public Mock<T> GetMock<T>() where T : class =>
            (Mock<T>)_mocks.GetOrAdd(typeof(T), new Mock<T>(_mockBehavior));

        /// <summary>
        /// Sets an instance of type <typeparamref name="T"/> to be used when resolving an object that needs <typeparamref name="T"/> 
        /// or when creating an object of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <param name="instance">The instance to use</param>
        public void SetInstance<T>(T instance) =>
            _container.Register(typeof(T), instance);

        /// <summary>
        /// Setup a void method of the mock that will be passed in replace of a dependency of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type of the mocked dependency</typeparam>
        /// <param name="expression">The method to mock</param>
        public ISetup<T> Setup<T>(Expression<Action<T>> expression) where T : class =>
            GetMock<T>().Setup(expression);

        /// <summary>
        /// Setup a void method of the mock that will be passed in replace of a dependency of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type of the mocked dependency</typeparam>
        /// <typeparam name="TResult">Type of the method result</typeparam>
        public ISetup<T, TResult> Setup<T, TResult>(Expression<Func<T, TResult>> expression) where T : class =>
            GetMock<T>().Setup(expression);

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// </summary>
        /// <typeparam name="T">Type of the mock</typeparam>
        /// <param name="expression">Expression to verify</param>
        public void Verify<T>(Expression<Action<T>> expression) where T : class =>
            GetMock<T>().Verify(expression);

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// </summary>
        /// <typeparam name="T">Type of the mock</typeparam>
        /// <param name="expression">Expression to verify</param>
        /// <param name="failMessage">Message to show if verification fails</param>
        public void Verify<T>(Expression<Action<T>> expression, string failMessage) where T : class =>
            GetMock<T>().Verify(expression, failMessage);

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// </summary>
        /// <typeparam name="T">Type of the mock</typeparam>
        /// <param name="expression">Expression to verify</param>
        /// <param name="times">The number of times the expression is expected to be called</param>
        public void Verify<T>(Expression<Action<T>> expression, Times times) where T : class =>
            GetMock<T>().Verify(expression, times);

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// </summary>
        /// <typeparam name="T">Type of the mock</typeparam>
        /// <param name="expression">Expression to verify</param>
        /// <param name="times">The number of times the expression is expected to be called</param>
        /// <param name="failMessage">Message to show if verification fails</param>
        public void Verify<T>(Expression<Action<T>> expression, Times times, string failMessage) where T : class =>
            GetMock<T>().Verify(expression, times, failMessage);
    }
}
