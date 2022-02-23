using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ThaudalAPI.Infrastructure.Interface;
using ThaudalAPI.Model.Model.Customer;

namespace ThaudalAPI.Infrastructure.Service;

public class CosmosCustomerRepository: ICustomerRepository
{
    private readonly ILogger<CosmosCustomerRepository> _logger;
    private readonly Container _container;

    public CosmosCustomerRepository(ILogger<CosmosCustomerRepository> logger, CosmosClient cosmosClient, IConfiguration configuration)
    {
        _logger = logger;
        _container = cosmosClient.GetContainer(configuration["Cosmos:Database"], configuration["Cosmos:Container"]);
    }
    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        throw new NotImplementedException();
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteCustomerAsync(string customerNumber)
    {
        throw new NotImplementedException();
    }

    public async Task<Customer?> GetCustomerAsync(string customerNumber)
    {
        throw new NotImplementedException();
    }

    public async IAsyncEnumerable<Customer> GetCustomersAsync(string? queryString = null)
    {
        var query = _container.GetItemQueryIterator<Customer>(new QueryDefinition(queryString));
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            foreach (var customer in response) yield return customer;
        }
    }

    public async IAsyncEnumerable<Customer> GetCustomerAsync(string? queryString, Dictionary<string, object> parameters)
    {
        var qd = new QueryDefinition(queryString);
        foreach (var (parameter, value) in parameters) qd.WithParameter(parameter, value);
        FeedIterator<Customer>? query;
        try
        {
            query = _container.GetItemQueryIterator<Customer>(qd, requestOptions: new QueryRequestOptions()
            {
                PartitionKey = new PartitionKey("/Id"),
                MaxItemCount = 1
            });
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "An error occurred");
            yield break;
        }
        
        
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            foreach (var customer in response)
            {
                _logger.LogInformation("User {User}", customer.CustomerNumber);
                yield return customer;
            }
        }
    }
}