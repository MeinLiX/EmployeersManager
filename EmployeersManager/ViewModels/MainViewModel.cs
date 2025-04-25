using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EmployeersManager.Core;
using EmployeersManager.Core.Enums;
using EmployeersManager.Core.Interfaces;
using EmployeersManager.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeersManager.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject _currentViewModel;

    [ObservableProperty]
    private bool _isMenuOpen;

    private readonly IDialogService _dialogService;
    private readonly IEmployeeRepository _employeeRepository;

    public MainViewModel()
    {
        _dialogService = App.Current.ServicesProvider.GetRequiredService<IDialogService>();
        _employeeRepository = App.Current.ServicesProvider.GetRequiredService<IEmployeeRepository>();

        CurrentViewModel = new EmployeeListViewModel();

        WeakReferenceMessenger.Default.Register<NavigationMessage>(this, (r, m) => {
            _ = m.ToNavigate switch
            {
                NavigationViewModel.EmloyeeList => CurrentViewModel = new EmployeeListViewModel(),
                NavigationViewModel.AddEmployee => CurrentViewModel = new AddEmployeeViewModel(),
                NavigationViewModel.Statistics => CurrentViewModel = new StatisticsViewModel(),
                NavigationViewModel.Positions => CurrentViewModel = new PositionsViewModel(),
                _ => new EmployeeListViewModel() // deault show list
            };
        });
        WeakReferenceMessenger.Default.Register<NavigationMessage<Position>>(this, (r, m) => {
            _ = m.ToNavigate switch
            {
                NavigationViewModel.PositionEdit => CurrentViewModel = new PositionEditViewModel(m.Arg),
                _ => new PositionsViewModel() // deault show list
            };
        });
    }

    [RelayCommand]
    private void NavigateToEmployeeList()
    {
        WeakReferenceMessenger.Default.Send(new NavigationMessage(NavigationViewModel.EmloyeeList));
        IsMenuOpen = false;
    }

    [RelayCommand]
    private void NavigateToAddEmployee()
    {
        WeakReferenceMessenger.Default.Send(new NavigationMessage(NavigationViewModel.AddEmployee));
        IsMenuOpen = false;
    }

    [RelayCommand]
    private void NavigateToStatistics()
    {
        WeakReferenceMessenger.Default.Send(new NavigationMessage(NavigationViewModel.Statistics));
        IsMenuOpen = false;
    }
    
    [RelayCommand]
    private void NavigateToPositions()
    {
        WeakReferenceMessenger.Default.Send(new NavigationMessage(NavigationViewModel.Positions));
        IsMenuOpen = false;
    }

    [RelayCommand]
    private async Task ShowExportDialog()
    {
        IsMenuOpen = false;
        await _dialogService.ShowExportDialogAsync();
    }

    [RelayCommand]
    private async Task ShowImportDialog()
    {
        IsMenuOpen = false;
        await _dialogService.ShowImportDialogAsync((IEnumerable<Employee> employees) =>
        {
            _employeeRepository.ImportRangeAsync(employees);
            NavigateToEmployeeList();
        });
    }
}
