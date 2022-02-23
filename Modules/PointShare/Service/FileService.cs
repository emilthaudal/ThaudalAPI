using Microsoft.Extensions.Logging;
using PointShare.Interface;
using ThaudalAPI.Infrastructure.Interface;
using ThaudalAPI.Model.Model.File;

namespace PointShare.Service;

public class FileService: IFileService
{
    private readonly ILogger<FileService> _logger;
    private readonly ICustomerRepository _customerRepository;

    public FileService(ILogger<FileService> logger, ICustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }
    public async Task<IEnumerable<FileDescriptor>> UploadFilesToCustomer(string customerNumber, IEnumerable<FileUploadInfo> uploadInfos)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<FileDescriptor>> UploadFilesToWorkCase(string customerNumber, string workCaseName, IEnumerable<FileUploadInfo> uploadInfos)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<FileDescriptor>> UpdateFilesCustomer(string customerNumber, IEnumerable<FileUploadInfo> uploadInfos)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<FileDescriptor>> UpdateFilesWorkCase(string customerNumber, string workCaseName, IEnumerable<FileUploadInfo> uploadInfos)
    {
        throw new NotImplementedException();
    }
}