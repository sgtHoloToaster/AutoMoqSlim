using AutoMoqSlim.Abstract;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AutoMoqSlim
{
    public class AutoMoqSlim
    {
        readonly ConcurrentDictionary<Type, Mock> _mocks = new();

        public T Create<T>()
        {
            throw new NotImplementedException();
        }

        public Mock<T> GetMock<T>() where T : class =>
            _mocks.GetOrAdd(typeof(T), new Mock<T>()).As<T>();

        public void SetInstance<T>(T instance)
        {
            throw new NotImplementedException();
        }
    }
}
