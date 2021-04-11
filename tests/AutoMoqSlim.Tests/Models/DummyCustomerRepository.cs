namespace AutoMoqSlim.Tests.Models
{
    public class DummyCustomerRepository : ICustomerRepository
    {
        public Customer? GetById(int id) => null;
    }
}
