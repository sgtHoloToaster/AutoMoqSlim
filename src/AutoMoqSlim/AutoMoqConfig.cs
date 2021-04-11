using Moq;

namespace AutoMoqSlim
{
    public class AutoMoqConfig
    {
        public MockBehavior MockBehavior { get; set; } = MockBehavior.Default;
    }
}
