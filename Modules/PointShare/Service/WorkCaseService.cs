using Microsoft.Extensions.Logging;
using PointShare.Interface;
using ThaudalAPI.Infrastructure.Interface;

namespace PointShare.Service;

public class WorkCaseService: IWorkCaseService
{
    private readonly ILogger<WorkCaseService> _logger;
    private readonly ICustomerRepository _customerRepository;

    public WorkCaseService(ILogger<WorkCaseService> logger, ICustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }
}