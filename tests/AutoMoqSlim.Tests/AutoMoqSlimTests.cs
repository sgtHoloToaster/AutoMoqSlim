using AutoMoqSlim.Tests.Models;
using Xunit;

namespace AutoMoqSlim.Tests
{
    public class AutoMoqSlimTests
    {
        [Fact]
        public void CanGetMock()
        {
            // arrange
            var target = new AutoMoqSlim();

            // act
            var mock = target.GetMock<ICustomerRepository>();

            // assert
            Assert.NotNull(mock);
        }

        [Fact]
        public void ReturnedMockSetupIsPreserved()
        {
            // arrange
            var target = new AutoMoqSlim();
            var expected = new Customer(1, "John");
            target.GetMock<ICustomerRepository>()
                .Setup(r => r.GetById(expected.Id))
                .Returns(expected);

            // act
            var result = target.GetMock<ICustomerRepository>()
                .Object
                .GetById(expected.Id);

            // assert
            Assert.Equal(expected, result);
        }
    }
}
