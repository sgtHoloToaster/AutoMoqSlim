using System;

namespace AutoMoqSlim.Exceptions
{
    public class CouldNotResolveParameterException : Exception
    {
        public Type TargetType { get; }

        public Type ParameterType { get; }

        public CouldNotResolveParameterException(Type targetType, Type parameterType) 
            : base($"Could not resolve a parameter of type {parameterType} when creating an instance of type {targetType}")
        {
            TargetType = targetType;
            ParameterType = parameterType;
        }
    }
}
