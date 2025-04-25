using EmployeersManager.Core.Models;

namespace EmployeersManager.Core.Interfaces;

public interface IDataExportService
{
    public Task ExportToExcelAsync(IEnumerable<Employee> employees, string filePath);

    public Task ExportToXmlAsync(IEnumerable<Employee> employees, string filePath);

    public Task ExportToJsonAsync(IEnumerable<Employee> employees, string filePath);
}
