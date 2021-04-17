using System;

namespace AutoMoqSlim
{
    public interface IContainer
    {
        bool IsRegistered(Type type);

        void Register(Type type, object? instance);

        bool TryResolve(Type type, out object? instance);
    }
}