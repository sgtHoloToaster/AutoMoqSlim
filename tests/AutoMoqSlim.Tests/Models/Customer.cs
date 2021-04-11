using System;

namespace AutoMoqSlim.Tests.Models
{
    public class Customer
    {
        public Customer(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }

        public override bool Equals(object? obj) =>
            obj is Customer customer &&
                   Id == customer.Id &&
                   Name == customer.Name;

        public override int GetHashCode() =>
            HashCode.Combine(Id, Name);
    }
}
