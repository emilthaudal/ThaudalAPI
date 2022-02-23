using ThaudalAPI.Model.Model.Customer;

namespace ThaudalAPI.Infrastructure.Interface;

public interface ICustomerRepository
{
    public Task<Customer> CreateCustomerAsync(Customer customer);
    public Task<Customer> UpdateCustomerAsync(Customer customer);
    public Task DeleteCustomerAsync(string customerNumber);
    public Task<Customer?> GetCustomerAsync(string customerNumber);
    public IAsyncEnumerable<Customer> GetCustomersAsync(string? query = null);

    public IAsyncEnumerable<Customer> GetCustomerAsync(string? queryString, Dictionary<string, object> parameters);

}