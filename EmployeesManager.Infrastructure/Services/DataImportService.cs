using EmployeesManager.Core.Interfaces.Services;
using EmployeesManager.Core.Models;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmployeesManager.Infrastructure.Services;

public class DataImportService : IDataImportService
{
    public Task<IEnumerable<Employee>> ImportFromExcelAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Employee>> ImportFromJsonAsync(string filePath)
    {
        return await Task.Run(() => {
            using var stream = File.OpenRead(filePath);
            return JsonSerializer.Deserialize<IEnumerable<Employee>>(stream,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
        });

    }

    public Task<IEnumerable<Employee>> ImportFromXmlAsync(string filePath)
    {
        throw new NotImplementedException();
    }
}
