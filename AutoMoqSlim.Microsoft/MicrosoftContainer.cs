using Microsoft.Extensions.DependencyInjection;
using System;

namespace AutoMoqSlim.Microsoft
{
    public class MicrosoftContainer : IContainer
    {
        readonly IServiceCollection _serviceCollection;
        IServiceProvider? _serviceProvider;

        public MicrosoftContainer() : this(new ServiceCollection())
        {
        }

        public MicrosoftContainer(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public bool IsRegistered(Type type) =>
            ServiceProvider.GetService(type) != null;

        public void Register(Type type, object? instance)
        {
            _serviceCollection.Add(new ServiceDescriptor(type, instance));
            _serviceProvider = null;
        }

        public bool TryResolve(Type type, out object instance)
        {
            instance = ServiceProvider.GetService(type);
            return instance != default;
        }

        IServiceProvider ServiceProvider => _serviceProvider ??= _serviceCollection.BuildServiceProvider();
    }
}
