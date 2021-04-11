﻿using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoMoqSlim
{
    public class AutoMoqer
    {
        readonly ConcurrentDictionary<Type, Mock> _mocks = new();
        readonly Dictionary<Type, object?> _registeredInstances = new();
        readonly AutoMoqerConfig _autoMoqConfig;

        public AutoMoqer()
        {
            _autoMoqConfig = new AutoMoqerConfig();
        }

        public AutoMoqer(AutoMoqerConfig autoMoqConfig)
        {
            _autoMoqConfig = autoMoqConfig;
        }

        public object Create(Type type)
        {
            if (_registeredInstances.TryGetValue(type, out var instance) && instance != null)
                return instance;
            else if (_mocks.TryGetValue(type, out var mock))
                return mock.Object;

            var constructor = GetConstructor(type);
            var parameters = constructor.GetParameters()
                .Select(p => _registeredInstances.TryGetValue(p.ParameterType, out var value) ? value : GetMock(p.ParameterType).Object)
                .ToArray();

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
                    || _registeredInstances.ContainsKey(p.ParameterType)))
                .Constructor;

        private Mock CreateMock(Type type)
        {
            var constructor = typeof(Mock<>).MakeGenericType(type).GetConstructor(new Type[] { typeof(MockBehavior) });
            return (Mock)constructor.Invoke(new object[] { _autoMoqConfig.MockBehavior });
        }

        public Mock GetMock(Type type) =>
            _mocks.GetOrAdd(type, CreateMock(type));

        public Mock<T> GetMock<T>() where T : class =>
            _mocks.GetOrAdd(typeof(T), new Mock<T>(_autoMoqConfig.MockBehavior)).As<T>();

        public void SetInstance<T>(T instance) =>
            _registeredInstances[typeof(T)] = instance;
    }
}
