using System.ComponentModel.DataAnnotations;
using ThaudalAPI.Model.Model.Case;
using ThaudalAPI.Model.Model.File;

namespace ThaudalAPI.Model.Model.Customer;

public class Customer
{
    [Key]
    public string CustomerNumber { get; set; }
    public string CustomerName { get; set; }
    public string RelationsEmployeeId { get; set; }
    public List<string> EmployeeAccess { get; set; }
    public List<FileDescriptor> CustomerFiles { get; set; }
    public List<WorkCase> WorkCases { get; set; }
}