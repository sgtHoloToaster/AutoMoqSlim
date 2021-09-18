using AutoMoqSlim.Exceptions;
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
            var target = new AutoMoqer();

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
            var target = new AutoMoqer();
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
            var target = new AutoMoqer();

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
            var target = new AutoMoqer();

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
            var target = new AutoMoqer();
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
            var target = new AutoMoqer();
            var expected = new DummyCustomerRepository();

            // act
            target.SetInstance<ICustomerRepository>(expected);
            var result = target.Create<CustomerRepositoryContainer>()
                .CustomerRepository;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SelectConstructorWithNonAbstractParameterIfItIsRegistered()
        {
            // arrange
            var target = new AutoMoqer();
            var expected = "It works";
            target.SetInstance(expected);

            // act
            var result = target.Create<CustomerRepositoryContainer>()
                .Name;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreatedMockBehaviorIsLooseByDefault()
        {
            // arrange
            var target = new AutoMoqer();

            // act
            var result = target.GetMock<ICustomerRepository>();

            // assert
            Assert.Equal(MockBehavior.Loose, result.Behavior);
        }

        [Fact]
        public void CreatedNonGenericMockBehaviorIsLooseByDefault()
        {
            // arrange
            var target = new AutoMoqer();

            // act
            var result = target.GetMock(typeof(ICustomerRepository));

            // assert
            Assert.Equal(MockBehavior.Loose, result.Behavior);
        }

        [Fact]
        public void CanBeConfiguredToCreateStrictMocks()
        {
            // arrange
            var target = new AutoMoqer(MockBehavior.Strict);

            // act
            var result = target.GetMock<ICustomerRepository>();

            // assert
            Assert.Equal(MockBehavior.Strict, result.Behavior);
        }

        [Fact]
        public void CanBeConfiguredToCreateStrictNonGenericMocks()
        {
            // arrange
            var target = new AutoMoqer(MockBehavior.Strict);

            // act
            var result = target.GetMock(typeof(ICustomerRepository));

            // assert
            Assert.Equal(MockBehavior.Strict, result.Behavior);
        }

        [Fact]
        public void CanCreateMockObject()
        {
            // arrange
            var target = new AutoMoqer();
            var expected = target.GetMock<ICustomerRepository>().Object;

            // act
            var result = target.Create<ICustomerRepository>();

            // assert
            Assert.Same(expected, result);
        }

        [Fact]
        public void CanCreateRegisteredInstance()
        {
            // arrange
            var target = new AutoMoqer();
            var expected = new DummyCustomerRepository();
            target.SetInstance<ICustomerRepository>(expected);

            // act
            var result = target.Create<ICustomerRepository>();

            // assert
            Assert.Same(expected, result);
        }

        [Fact]
        public void NoPublicConstructorExceptionIsThrownWhenClassHasNoPublicConstructor()
        {
            // arrange
            var target = new AutoMoqer();

            // act & assert
            Assert.Throws<NoPublicConstructorException>(target.Create<ClassWithoutPublicConstructor>);
        }

        [Fact]
        public void CanMockAbstractClasses()
        {
            // arrange
            var target = new AutoMoqer();

            // act
            var result = target.GetMock<AbstractCustomerRepository>();

            // assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Mock<AbstractCustomerRepository>>(result);
        }
    }
}
