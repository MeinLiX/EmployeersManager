using EmployeesManager.Core.Models;

namespace EmployeesManager.Core.Interfaces.Services;

public interface IDataImportService
{
    public Task<IEnumerable<Employee>> ImportFromExcelAsync(string filePath);

    public Task<IEnumerable<Employee>> ImportFromXmlAsync(string filePath);

    public Task<IEnumerable<Employee>> ImportFromJsonAsync(string filePath);
}
