namespace AutoMoqSlim.Tests.Models
{
    public class CustomerRepositoryContainer
    {
        public CustomerRepositoryContainer(ICustomerRepository customerRepository)
        {
            CustomerRepository = customerRepository;
        }

        public ICustomerRepository CustomerRepository { get; }
    }
}
