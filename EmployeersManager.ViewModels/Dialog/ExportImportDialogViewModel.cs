using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeersManager.Core.Enums;
using EmployeersManager.Core.Interfaces;
using EmployeersManager.Core.Models;
using Microsoft.Win32;

namespace EmployeersManager.ViewModels.Dialog;

public partial class ExportImportDialogViewModel : ObservableObject, IDialogViewModel
{
    private readonly IDataExportService _exportService;
    private readonly IDataImportService _importService;
    private readonly IEnumerable<Employee> _employees;
    private readonly Action<IEnumerable<Employee>> _importCallback;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private bool _isExportMode;

    [ObservableProperty]
    private string _filePath;

    [ObservableProperty]
    private object _selectedFileType;

    public IEnumerable<object> FileTypes { get; }

    public bool CanProcess => !string.IsNullOrWhiteSpace(FilePath) && SelectedFileType != null;

    public ExportImportDialogViewModel(
        bool isExportMode,
        IDataExportService exportService,
        IDataImportService importService,
        IDialogService dialogService,
        IEnumerable<Employee> employees = null,
        Action<IEnumerable<Employee>> importCallback = null)
    {
        _isExportMode = isExportMode;
        _exportService = exportService;
        _importService = importService;
        _dialogService = dialogService;
        _employees = employees;
        _importCallback = importCallback;

        FileTypes = isExportMode
            ? Enum.GetValues(typeof(ExportType)).Cast<object>().ToList()
            : Enum.GetValues(typeof(ImportType)).Cast<object>().ToList();

        SelectedFileType = isExportMode ? ExportType.JSON : ImportType.JSON;

        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(FilePath) || e.PropertyName == nameof(SelectedFileType))
            {
                OnPropertyChanged(nameof(CanProcess));
            }
        };
    }

    [RelayCommand]
    private void BrowseFile()
    {
        if (IsExportMode)
        {
            var dialog = new SaveFileDialog();
            SetFileDialogFilter(dialog);

            if (dialog.ShowDialog() == true)
            {
                FilePath = dialog.FileName;
            }
        }
        else
        {
            var dialog = new OpenFileDialog();
            SetFileDialogFilter(dialog);

            if (dialog.ShowDialog() == true)
            {
                FilePath = dialog.FileName;
            }
        }
    }

    private void SetFileDialogFilter(FileDialog dialog)
    {
        switch (SelectedFileType)
        {
            case ExportType.EXCEL:
            case ImportType.EXCEL:
                dialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                dialog.DefaultExt = ".xlsx";
                break;
            case ExportType.XML:
            case ImportType.XML:
                dialog.Filter = "XML Files (*.xml)|*.xml";
                dialog.DefaultExt = ".xml";
                break;
            case ExportType.JSON:
            case ImportType.JSON:
                dialog.Filter = "JSON Files (*.json)|*.json";
                dialog.DefaultExt = ".json";
                break;
        }
    }

    [RelayCommand]
    private async Task Process()
    {
        try
        {
            if (IsExportMode)
            {
                await ExportData();
            }
            else
            {
                await ImportData();
            }

            _dialogService.CloseDialog();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }

    private async Task ExportData()
    {
        if (_employees == null)
            return;
        try
        {
            switch (SelectedFileType)
            {
                case ExportType.EXCEL:
                    await _exportService.ExportToExcelAsync(_employees, FilePath);
                    break;
                case ExportType.XML:
                    await _exportService.ExportToXmlAsync(_employees, FilePath);
                    break;
                case ExportType.JSON:
                    await _exportService.ExportToJsonAsync(_employees, FilePath);
                    break;
            }
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }

    private async Task ImportData()
    {
        IEnumerable<Employee> importedEmployees = null;
        try
        {
            switch (SelectedFileType)
            {
                case ImportType.EXCEL:
                    importedEmployees = await _importService.ImportFromExcelAsync(FilePath);
                    break;
                case ImportType.XML:
                    importedEmployees = await _importService.ImportFromXmlAsync(FilePath);
                    break;
                case ImportType.JSON:
                    importedEmployees = await _importService.ImportFromJsonAsync(FilePath);
                    break;
            }

            _importCallback?.Invoke(importedEmployees);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _dialogService.CloseDialog();
    }
}
