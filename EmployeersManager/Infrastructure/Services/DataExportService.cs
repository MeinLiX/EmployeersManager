using EmployeersManager.Core.Interfaces;
using EmployeersManager.Core.Models;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmployeersManager.Infrastructure.Services;

public class DataExportService : IDataExportService
{
    public Task ExportToExcelAsync(IEnumerable<Employee> employees, string filePath)
    {
        throw new NotImplementedException();
    }

    public Task ExportToXmlAsync(IEnumerable<Employee> employees, string filePath)
    {
        throw new NotImplementedException();
    }

    public async Task ExportToJsonAsync(IEnumerable<Employee> employees, string filePath)
    {
        await Task.Run(() => {
            string json = JsonSerializer.Serialize(employees,
                new JsonSerializerOptions { WriteIndented = true, ReferenceHandler = ReferenceHandler.Preserve });
            File.WriteAllText(filePath, json);
        });
    }
}
