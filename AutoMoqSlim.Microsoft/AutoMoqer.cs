using Moq;
using System;

namespace AutoMoqSlim.Microsoft
{
    public class AutoMoqer : AutoMoqSlim.AutoMoqer
    {
        public AutoMoqer(MockBehavior mockBehavior = MockBehavior.Default) : base(new MicrosoftContainer(), mockBehavior)
        {
        }
    }
}
