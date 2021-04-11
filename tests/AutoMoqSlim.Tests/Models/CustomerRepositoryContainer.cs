namespace AutoMoqSlim.Tests.Models
{
    public class CustomerRepositoryContainer
    {
        public CustomerRepositoryContainer(ICustomerRepository customerRepository, string name) : this(customerRepository)
        {

        }

        public CustomerRepositoryContainer(ICustomerRepository customerRepository)
        {
            CustomerRepository = customerRepository;
        }

        public ICustomerRepository CustomerRepository { get; }
    }
}
