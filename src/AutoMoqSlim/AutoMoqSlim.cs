using AutoMoqSlim.Abstract;
using Moq;
using System;

namespace AutoMoqSlim
{
    public class AutoMoqSlim
    {
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
