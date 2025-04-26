using EmployeersManager.Core.Models;
using System.Windows.Controls;

namespace EmployeersManager.Core.Interfaces;

public interface IDialogService
{
    Task<bool> ShowExportDialogAsync(Func<IDialogViewModel, IDialogView> initialView);
    Task<bool> ShowImportDialogAsync(Action<IEnumerable<Employee>> importCallback, Func<IDialogViewModel, IDialogView> initialView);

    void CloseDialog(bool result = false);
}
