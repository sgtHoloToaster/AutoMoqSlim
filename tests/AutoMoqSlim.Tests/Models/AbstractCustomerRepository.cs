namespace AutoMoqSlim.Tests.Models
{
    public abstract class AbstractCustomerRepository : ICustomerRepository
    {
        public abstract Customer? GetById(int id);
    }
}
