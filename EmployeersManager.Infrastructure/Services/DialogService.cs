using EmployeersManager.Core.Interfaces;
using EmployeersManager.Core.Models;
using EmployeersManager.ViewModels.Dialog;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace EmployeersManager.Infrastructure.Services;

public class DialogService : IDialogService
{
    private readonly IDataExportService _exportService;
    private readonly IDataImportService _importService;
    private readonly IEmployeeRepository _employeeRepository;
    private const string DialogIdentifier = "RootDialog";

    public DialogService(IDataExportService exportService, IDataImportService importService, IEmployeeRepository employeeRepository)
    {
        _exportService = exportService;
        _importService = importService;
        _employeeRepository = employeeRepository;
    }

    public async Task<bool> ShowExportDialogAsync(Func<IDialogViewModel, IDialogView> initialView)
    {
        var dialogViewModel = new ExportImportDialogViewModel(
            isExportMode: true,
            exportService: _exportService,
            importService: _importService,
            dialogService: this,
            employees: await _employeeRepository.GetAllAsync());

        var view = initialView.Invoke(dialogViewModel);

        var result = await DialogHost.Show(view, DialogIdentifier);
        return result is bool boolResult && boolResult;
    }

    public async Task<bool> ShowImportDialogAsync(Action<IEnumerable<Employee>> importCallback, Func<IDialogViewModel, IDialogView> initialView)
    {
        var dialogViewModel = new ExportImportDialogViewModel(
            isExportMode: false,
            exportService: _exportService,
            importService: _importService,
            dialogService: this,
            importCallback: importCallback);

        var view = initialView.Invoke(dialogViewModel);

        var result = await DialogHost.Show(view, DialogIdentifier);
        return result is bool boolResult && boolResult;
    }

    public void CloseDialog(bool result = false)
    {
        if (DialogHost.IsDialogOpen(DialogIdentifier))
        {
            DialogHost.Close(DialogIdentifier, result);
        }
    }
}