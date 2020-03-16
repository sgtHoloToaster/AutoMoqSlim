using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMoq.Abstract
{
    public interface IContainer
    {
        T Resolve<T>();
        object Resolve(Type type);
        void Register<T>(T instance);
        void Register(object instance, Type type);
    }
}
