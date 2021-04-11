namespace AutoMoqSlim.Tests.Models
{
    public class CustomerRepositoryContainer
    {
        public CustomerRepositoryContainer(ICustomerRepository customerRepository)
        {
            CustomerRepository = customerRepository;
        }

        public CustomerRepositoryContainer(ICustomerRepository customerRepository, string name) : this(customerRepository)
        {
            Name = name;
        }

        public ICustomerRepository CustomerRepository { get; }

        public string? Name { get; }
    }
}
