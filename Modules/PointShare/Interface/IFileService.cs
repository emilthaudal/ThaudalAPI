using ThaudalAPI.Model.Model.File;

namespace PointShare.Interface;

public interface IFileService
{
    public Task<IEnumerable<FileDescriptor>> UploadFilesToCustomer(string customerNumber, IEnumerable<FileUploadInfo> uploadInfos);

    public Task<IEnumerable<FileDescriptor>> UploadFilesToWorkCase(string customerNumber, string workCaseName, IEnumerable<FileUploadInfo> uploadInfos);

    public Task<IEnumerable<FileDescriptor>>UpdateFilesCustomer(string customerNumber,
        IEnumerable<FileUploadInfo> uploadInfos);
    public Task<IEnumerable<FileDescriptor>> UpdateFilesWorkCase(string customerNumber, string workCaseName,
        IEnumerable<FileUploadInfo> uploadInfos);
}