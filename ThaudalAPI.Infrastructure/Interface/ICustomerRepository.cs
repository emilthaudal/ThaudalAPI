using ThaudalAPI.Model.Model.Customer;

namespace ThaudalAPI.Infrastructure.Interface;

public interface ICustomerRepository
{
    public Task<Customer> CreateCustomer(Customer customer);
    public Task<Customer> UpdateCustomer(Customer customer);
    public Task<bool> DeleteCustomer(string customerNumber);
}