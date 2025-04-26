using EmployeesManager.Core.AggregationModels;
using EmployeesManager.Core.Models;

namespace EmployeesManager.Core.Interfaces.Repository;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IEnumerable<Employee>> SearchAsync(string searchTerm);
    Task<bool> TerminateAsync(int id);
    Task<EmployeeStatistics> GetStatisticsAsync();

    Task ImportRangeAsync(IEnumerable<Employee> employees);
}
