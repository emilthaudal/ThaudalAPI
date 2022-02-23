using Microsoft.Extensions.Logging;
using PointShare.Interface;
using ThaudalAPI.Infrastructure.Interface;

namespace PointShare.Service;

public class TaskService: ITaskService
{
    private readonly ILogger<TaskService> _logger;
    private readonly ICustomerRepository _customerRepository;

    public TaskService(ILogger<TaskService> logger, ICustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }
}