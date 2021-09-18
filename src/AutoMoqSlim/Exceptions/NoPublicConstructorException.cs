using System;

namespace AutoMoqSlim.Exceptions
{
    public class NoPublicConstructorException : Exception
    {
        public NoPublicConstructorException(Type type) : base($"No public constructor is registered for type {type.FullName}") { }
    }
}
