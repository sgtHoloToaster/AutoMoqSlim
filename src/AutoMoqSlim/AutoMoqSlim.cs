using AutoMoqSlim.Abstract;
using Moq;
using System;

namespace AutoMoqSlim
{
    public class AutoMoqSlim
    {
        readonly IContainer _container;

        public AutoMoqSlim(IContainer container)
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
