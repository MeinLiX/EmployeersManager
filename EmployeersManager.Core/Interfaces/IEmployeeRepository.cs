using EmployeersManager.Core.AggregationModels;
using EmployeersManager.Core.Models;

namespace EmployeersManager.Core.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IEnumerable<Employee>> SearchAsync(string searchTerm);
    Task<bool> TerminateAsync(int id);
    Task<EmployeeStatistics> GetStatisticsAsync();

    Task ImportRangeAsync(IEnumerable<Employee> employees);
}
