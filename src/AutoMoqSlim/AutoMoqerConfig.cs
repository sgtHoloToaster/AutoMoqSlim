using Moq;

namespace AutoMoqSlim
{
    public class AutoMoqerConfig
    {
        public MockBehavior MockBehavior { get; set; } = MockBehavior.Default;

        public Container Container { get; set; } = new Container();
    }
}
