using ThaudalAPI.Infrastructure.Interface;
using ThaudalAPI.Model.Model.Employee;

namespace ThaudalAPI.Infrastructure.Service;

public class CosmosEmployeeRepository: IEmployeeRepository
{
    public async Task<Employee> CreateEmployee(Employee employee)
    {
        throw new NotImplementedException();
    }

    public async Task<Employee> UpdateEmployee(Employee employee)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteEmployee(string id)
    {
        throw new NotImplementedException();
    }
}