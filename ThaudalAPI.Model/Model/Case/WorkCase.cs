using System.ComponentModel.DataAnnotations;
using ThaudalAPI.Model.Model.File;
using ThaudalAPI.Model.Model.Task;

namespace ThaudalAPI.Model.Model.Case;

public class WorkCase
{
    [Key]
    public string Name { get; set; }
    public int CaseYear { get; set; }
    public List<CaseTask> CaseTasks { get; set; }
    public List<FileDescriptor> CaseFiles { get; set; }
}