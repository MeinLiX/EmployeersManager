using EmployeersManager.Core.Models;

namespace EmployeersManager.Core.Interfaces;

public interface IDataImportService
{
    public Task<IEnumerable<Employee>> ImportFromExcelAsync(string filePath);

    public Task<IEnumerable<Employee>> ImportFromXmlAsync(string filePath);

    public Task<IEnumerable<Employee>> ImportFromJsonAsync(string filePath);
}
