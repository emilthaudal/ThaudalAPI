using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ThaudalAPI.Infrastructure.Interface;
using User = ThaudalAPI.Model.Model.Users.User;

namespace ThaudalAPI.Infrastructure.Service;

public class CosmosUserRepository : IUserRepository
{
    private readonly IConfiguration _configuration;
    private readonly Container _container;
    private readonly CosmosClient _cosmosClient;
    private readonly ILogger<CosmosUserRepository> _logger;

    public CosmosUserRepository(ILogger<CosmosUserRepository> logger, CosmosClient cosmosClient,
        IConfiguration configuration)
    {
        _logger = logger;
        _cosmosClient = cosmosClient;
        _configuration = configuration;
        _container = cosmosClient.GetContainer(_configuration["Cosmos:Database"], _configuration["Cosmos:Container"]);
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        try
        {
            var response = await _container.ReadItemAsync<User>(id.ToString(), new PartitionKey(id.ToString()));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        var parameters = new Dictionary<string, object>
        {
            {
                "@Username", username
            }
        };
        var user = await GetUsersAsync("select * from c where c.Username = @Username", parameters)
            .FirstOrDefaultAsync();
        return user;
    }

    public async Task<User?> GetUserByRefreshToken(string refreshToken)
    {
        var parameters = new Dictionary<string, object>
        {
            {
                "@token", refreshToken
            }
        };
        var user = await GetUsersAsync("select c.Id from c join c.RefreshToken as t where t.Token = @token", parameters)
            .FirstOrDefaultAsync();
        return user;
    }

    public async IAsyncEnumerable<User> GetUsersAsync(string? queryString)
    {
        var query = _container.GetItemQueryIterator<User>(new QueryDefinition(queryString));
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            foreach (var user in response) yield return user;
        }
    }

    public async IAsyncEnumerable<User> GetUsersAsync(string? queryString, Dictionary<string, object> parameters)
    {
        var qd = new QueryDefinition(queryString);
        foreach (var parameter in parameters) qd.WithParameter(parameter.Key, parameter.Value);
        var query = _container.GetItemQueryIterator<User>(qd);
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            foreach (var user in response) yield return user;
        }
    }

    public async Task<User?> CreateUserAsync(User user)
    {
        if (user.Id == default)
        {
            user.Id = Guid.NewGuid();
        }
        var response = await _container.CreateItemAsync(user, new PartitionKey(user.Id.ToString()));
        return response.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created ? response.Resource : null;
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        var response = await _container.UpsertItemAsync(user, new PartitionKey(user.Id.ToString()));
        return response.Resource;
    }
}