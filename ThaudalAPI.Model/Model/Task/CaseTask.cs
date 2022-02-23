using System.ComponentModel.DataAnnotations;

namespace ThaudalAPI.Model.Model.Task;

public class CaseTask
{
    [Key]
    public string Name { get; set; }
    public TaskState State { get; set; }
}