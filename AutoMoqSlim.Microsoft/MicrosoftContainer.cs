using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMoqSlim.Microsoft
{
    public class MicrosoftContainer : IContainer
    {
        public bool IsRegistered(Type type)
        {
            throw new NotImplementedException();
        }

        public void Register(Type type, object instance)
        {
            throw new NotImplementedException();
        }

        public bool TryResolve(Type type, out object instance)
        {
            throw new NotImplementedException();
        }
    }
}
