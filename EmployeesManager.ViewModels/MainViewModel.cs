using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EmployeesManager.Core;
using EmployeesManager.Core.Enums;
using EmployeesManager.Core.Interfaces;
using EmployeesManager.Core.Interfaces.Repository;
using EmployeesManager.Core.Interfaces.Services;
using EmployeesManager.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeesManager.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject _currentViewModel;

    [ObservableProperty]
    private bool _isMenuOpen;

    private readonly IDialogService _dialogService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPositionRepository _positionRepository;
    private readonly IDialogView _exportImportDialog;

    public MainViewModel(IDialogService dialogService, 
                         IEmployeeRepository employeeRepository, 
                         IPositionRepository positionRepository,
                         [FromKeyedServices(DialogViews.ExportImportDialog)] IDialogView exportImportDialog)
    {
        _dialogService = dialogService;
        _employeeRepository = employeeRepository;
        _positionRepository = positionRepository;
        _exportImportDialog = exportImportDialog;

        CurrentViewModel = new EmployeeListViewModel(_employeeRepository);

        WeakReferenceMessenger.Default.Register<NavigationMessage>(this, (r, m) => {
            _ = m.ToNavigate switch
            {
                ViewModelNavigation.EmloyeeList => CurrentViewModel = new EmployeeListViewModel(_employeeRepository),
                ViewModelNavigation.AddEmployee => CurrentViewModel = new AddEmployeeViewModel(_employeeRepository, _positionRepository),
                ViewModelNavigation.Statistics => CurrentViewModel = new StatisticsViewModel(_employeeRepository, _positionRepository),
                ViewModelNavigation.Positions => CurrentViewModel = new PositionsViewModel(_positionRepository),
                _ => new EmployeeListViewModel(_employeeRepository) // deault show list
            };
        });
        WeakReferenceMessenger.Default.Register<NavigationMessage<Position>>(this, (r, m) => {
            _ = m.ToNavigate switch
            {
                ViewModelNavigation.PositionEdit => CurrentViewModel = new PositionEditViewModel(_positionRepository, m.Arg),
                _ => new PositionsViewModel(_positionRepository) // deault show list
            };
        });
    }

    [RelayCommand]
    private void NavigateToEmployeeList()
    {
        WeakReferenceMessenger.Default.Send(new NavigationMessage(ViewModelNavigation.EmloyeeList));
        IsMenuOpen = false;
    }

    [RelayCommand]
    private void NavigateToAddEmployee()
    {
        WeakReferenceMessenger.Default.Send(new NavigationMessage(ViewModelNavigation.AddEmployee));
        IsMenuOpen = false;
    }

    [RelayCommand]
    private void NavigateToStatistics()
    {
        WeakReferenceMessenger.Default.Send(new NavigationMessage(ViewModelNavigation.Statistics));
        IsMenuOpen = false;
    }
    
    [RelayCommand]
    private void NavigateToPositions()
    {
        WeakReferenceMessenger.Default.Send(new NavigationMessage(ViewModelNavigation.Positions));
        IsMenuOpen = false;
    }

    [RelayCommand]
    private async Task ShowExportDialog()
    {
        IsMenuOpen = false;
        await _dialogService.ShowExportDialogAsync(_exportImportDialog.BindViewModel);
    }

    [RelayCommand]
    private async Task ShowImportDialog()
    {
        IsMenuOpen = false;
        await _dialogService.ShowImportDialogAsync((IEnumerable<Employee> employees) =>
        {
            _employeeRepository.ImportRangeAsync(employees);
            NavigateToEmployeeList();
        }, _exportImportDialog.BindViewModel);
    }
}
