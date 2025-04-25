using EmployeersManager.Core.Models;

namespace EmployeersManager.Core.Interfaces;

public interface IDialogService
{
    Task<bool> ShowExportDialogAsync();
    Task<bool> ShowImportDialogAsync(Action<IEnumerable<Employee>> importCallback);

    void CloseDialog(bool result = false);
}
