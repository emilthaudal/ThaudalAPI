using ThaudalAPI.Model.Model.Employee;

namespace ThaudalAPI.Infrastructure.Interface;

public interface IEmployeeRepository
{
    public Task<Employee> CreateEmployee(Employee employee);
    public Task<Employee> UpdateEmployee(Employee employee);
    public Task DeleteEmployee(string id);
}