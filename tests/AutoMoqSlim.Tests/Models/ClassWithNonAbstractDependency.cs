namespace AutoMoqSlim.Tests.Models
{
    public class ClassWithNonAbstractDependency
    {
        public ClassWithNonAbstractDependency(DummyCustomerRepository customerRepository)
        {

        }
    }
}
