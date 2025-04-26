using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeesManager.Core.Interfaces.Repository;
using EmployeesManager.Core.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace EmployeesManager.ViewModels;

public partial class EmployeeListViewModel : ObservableObject
{
    private readonly IEmployeeRepository _repository;

    [ObservableProperty]
    private ObservableCollection<Employee> _employees;

    [ObservableProperty]
    private string _searchTerm = string.Empty;

    [ObservableProperty]
    private Employee _selectedEmployee;

    public EmployeeListViewModel(IEmployeeRepository employeeRepository)
    {
        _repository = employeeRepository;
        LoadEmployeesCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadEmployeesAsync()
    {
        var employees = await _repository.GetAllAsync();
        Employees = [.. employees];
    }

    [RelayCommand]
    private async Task Search()
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
        {
            await LoadEmployeesAsync();
            return;
        }

        var searchResults = await _repository.SearchAsync(SearchTerm);
        Employees = [.. searchResults];
    }

    [RelayCommand]
    private async Task TerminateEmployee(Employee employee)
    {
        if (employee != null)
        {
            await _repository.TerminateAsync(employee.Id);
            await LoadEmployeesAsync();
        }
    }

    [RelayCommand]
    private void ViewEmployeeDetails(Employee employee)
    {
        MessageBox.Show("Not implement");
    }

    [RelayCommand]
    private async Task DeleteEmployee(Employee employee)
    {
        if (employee != null)
        {
            await _repository.DeleteAsync(employee.Id);
            await LoadEmployeesAsync();
        }
    }
}
