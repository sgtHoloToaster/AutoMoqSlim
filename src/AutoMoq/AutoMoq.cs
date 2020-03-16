using AutoMoq.Abstract;
using Moq;
using System;

namespace AutoMoq
{
    public class AutoMoq
    {
        readonly IContainer _container;

        public AutoMoq(IContainer container)
        {
            _container = container;
        }

        public T Create<T>()
        {
            
            throw new NotImplementedException();
        }

        public Mock<T> GetMock<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void SetInstance<T>(T instance)
        {
            throw new NotImplementedException();
        }


    }
}
