namespace AutoMoqSlim.Tests.Models
{
    public interface ICustomerRepository
    {
        Customer? GetById(int id);
    }
}
