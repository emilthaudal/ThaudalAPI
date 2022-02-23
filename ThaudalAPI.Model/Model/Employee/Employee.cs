using System.ComponentModel.DataAnnotations;

namespace ThaudalAPI.Model.Model.Employee;

public class Employee
{
    [Key]
    public string Id { get; set; }
    public string UserId { get; set; }
}