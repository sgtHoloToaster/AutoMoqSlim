using System;

namespace AutoMoqSlim
{
    public interface IContainer
    {
        /// <summary>
        /// Checks if an instance of the type is registered in the container
        /// </summary>
        /// <param name="type">Type to check</param>
        bool IsRegistered(Type type);

        /// <summary>
        /// Registers the provided instance as the type
        /// </summary>
        /// <param name="type">Type to register the instance as</param>
        /// <param name="instance">Instance to register</param>
        void Register(Type type, object? instance);

        /// <summary>
        /// Tries to resolve an instance for the type. A return value indicates whether the operation succeeded
        /// </summary>
        /// <param name="type">Type to resolve</param>
        /// <param name="instance">Resolved instance. Null if there was no instance for the type in the container</param>
        /// <returns>Flag that indicates whether the operation succeeded</returns>
        bool TryResolve(Type type, out object? instance);
    }
}