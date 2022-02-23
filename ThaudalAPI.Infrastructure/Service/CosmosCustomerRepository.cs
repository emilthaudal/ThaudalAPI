using ThaudalAPI.Infrastructure.Interface;
using ThaudalAPI.Model.Model.Customer;

namespace ThaudalAPI.Infrastructure.Service;

public class CosmosCustomerRepository: ICustomerRepository
{
    public async Task<Customer> CreateCustomer(Customer customer)
    {
        throw new NotImplementedException();
    }

    public async Task<Customer> UpdateCustomer(Customer customer)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteCustomer(string customerNumber)
    {
        throw new NotImplementedException();
    }
}