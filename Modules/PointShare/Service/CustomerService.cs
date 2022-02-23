using Microsoft.Extensions.Logging;
using PointShare.Interface;
using ThaudalAPI.Infrastructure.Interface;
using ThaudalAPI.Model.Model.Customer;

namespace PointShare.Service;

public class CustomerService: ICustomerService
{
    private readonly ILogger<CustomerService> _logger;
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ILogger<CustomerService> logger, ICustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }
    public async Task<Customer> CreateCustomer(Customer customer)
    {
        var existing = await _customerRepository.GetCustomerAsync(customer.CustomerNumber);
        if (existing != default)
        {
            throw new InvalidOperationException("Customer already exists");
        }

        return await _customerRepository.CreateCustomerAsync(customer);
    }

    public async Task<Customer> UpdateCustomer(Customer customer)
    {
        throw new NotImplementedException();
    }
}