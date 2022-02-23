using ThaudalAPI.Model.Model.Customer;

namespace PointShare.Interface;

public interface ICustomerService
{
    public Task<Customer> CreateCustomer(Customer customer);
    public Task<Customer> UpdateCustomer(Customer customer);
}