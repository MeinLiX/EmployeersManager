using EmployeersManager.Core.Interfaces;
using EmployeersManager.Core.Models;
using EmployeersManager.Dialog;
using EmployeersManager.ViewModels.Dialog;
using MaterialDesignThemes.Wpf;

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

    public async Task<bool> ShowExportDialogAsync()
    {
        var dialogViewModel = new ExportImportDialogViewModel(
            isExportMode: true,
            exportService: _exportService,
            importService: _importService,
            dialogService: this,
            employees: await _employeeRepository.GetAllAsync());

        var view = new ExportImportDialog
        {
            DataContext = dialogViewModel
        };

        var result = await DialogHost.Show(view, DialogIdentifier);
        return result is bool boolResult && boolResult;
    }

    public async Task<bool> ShowImportDialogAsync(Action<IEnumerable<Employee>> importCallback)
    {
        var dialogViewModel = new ExportImportDialogViewModel(
            isExportMode: false,
            exportService: _exportService,
            importService: _importService,
            dialogService: this,
            importCallback: importCallback);

        var view = new ExportImportDialog
        {
            DataContext = dialogViewModel
        };

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