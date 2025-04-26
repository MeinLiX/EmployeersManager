using EmployeesManager.Core.Models;

namespace EmployeesManager.Core.Interfaces.Services;

public interface IDataExportService
{
    public Task ExportToExcelAsync(IEnumerable<Employee> employees, string filePath);

    public Task ExportToXmlAsync(IEnumerable<Employee> employees, string filePath);

    public Task ExportToJsonAsync(IEnumerable<Employee> employees, string filePath);
}
