using AutoMoqSlim.Tests.Models;
using Moq;
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
            Assert.IsType<Mock<ICustomerRepository>>(mock);
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

        [Fact]
        public void CanCreateInstance()
        {
            // arrange
            var target = new AutoMoqSlim();

            // act
            var result = target.Create<CustomerRepositoryContainer>();

            // assert
            Assert.NotNull(result);
            Assert.IsType<CustomerRepositoryContainer>(result);
        }

        [Fact]
        public void CreatedInstanceHasDefaultMock()
        {
            // arrange
            var target = new AutoMoqSlim();

            // act
            var result = target.Create<CustomerRepositoryContainer>();

            // assert
            Assert.NotNull(result.CustomerRepository);
            Assert.IsAssignableFrom<ICustomerRepository>(result.CustomerRepository);
        }

        [Fact]
        public void CreatedInstanceHasRegisteredMock()
        {
            // arrange
            var target = new AutoMoqSlim();
            var expected = new Customer(1, "John");
            target.GetMock<ICustomerRepository>()
                .Setup(r => r.GetById(expected.Id))
                .Returns(expected);

            // act
            var result = target.Create<CustomerRepositoryContainer>()
                .CustomerRepository
                .GetById(expected.Id);

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreatedInstanceHasRegisteredDependency()
        {
            // arrange
            var target = new AutoMoqSlim();
            var expected = new DummyCustomerRepository();

            // act
            target.SetInstance<ICustomerRepository>(expected);
            var result = target.Create<CustomerRepositoryContainer>()
                .CustomerRepository;

            // assert
            Assert.Equal(expected, result);
        }
    }
}
