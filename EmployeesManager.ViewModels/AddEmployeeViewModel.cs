using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EmployeesManager.Core;
using EmployeesManager.Core.Enums;
using EmployeesManager.Core.Interfaces.Repository;
using EmployeesManager.Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.ViewModels;

public partial class AddEmployeeViewModel : ObservableValidator
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPositionRepository _positionRepository;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "ПІБ не може бути порожнім")]
    [MinLength(5, ErrorMessage = "ПІБ має містити принаймні 5 символів")]
    [MaxLength(100, ErrorMessage = "ПІБ не може перевищувати 100 символів")]
    private string _fullName = string.Empty;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Потрібно обрати посаду")]
    private Position _position = null;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0.01, double.MaxValue, ErrorMessage = "Вкажіть зарплатню")]
    private decimal _salary;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(DateValidator), nameof(DateValidator.ValidateHireDate))]
    private DateTime _hireDate = DateTime.Today;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isSuccess = true;

    [ObservableProperty]
    private ObservableCollection<Position> _positions;

    public AddEmployeeViewModel(IEmployeeRepository employeeRepository, IPositionRepository positionRepository)
    {
        _employeeRepository = employeeRepository;
        _positionRepository = positionRepository;

        Task.Run(async () =>
        {
            Positions = [.. await _positionRepository.GetAllAsync()];
        }, cancellationToken: default); //can determine cancellation token
    }

    [RelayCommand]
    private async Task SaveEmployee()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            StatusMessage = "Будь ласка, виправте помилки перед збереженням";
            IsSuccess = false;
            return;
        }

        var employee = new Employee
        {
            FullName = FullName,
            PositionId = Position.Id,
            Salary = Salary,
            HireDate = HireDate
        };

        try
        {
            await _employeeRepository.AddAsync(employee);

            StatusMessage = "Співробітника успішно додано";
            IsSuccess = true;

            ClearForm();

            WeakReferenceMessenger.Default.Send(new NavigationMessage(ViewModelNavigation.EmloyeeList));
        }
        catch (Exception ex)
        {
            StatusMessage = $"Помилка: {ex.Message}";
            IsSuccess = false;
        }
    }

    [RelayCommand]
    private void ClearForm()
    {
        FullName = string.Empty;
        Position = null;
        Salary = 0;
        HireDate = DateTime.Today;
        StatusMessage = string.Empty;
    }
}

public static class DateValidator
{
    public static ValidationResult ValidateHireDate(DateTime date, ValidationContext context)
    {
        if (date > DateTime.Today)
        {
            return new ValidationResult("Дата прийняття не може бути у майбутньому");
        }

        return ValidationResult.Success;
    }
}
