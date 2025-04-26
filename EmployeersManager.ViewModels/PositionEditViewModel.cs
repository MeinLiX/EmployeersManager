using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EmployeersManager.Core;
using EmployeersManager.Core.Enums;
using EmployeersManager.Core.Interfaces;
using EmployeersManager.Core.Models;
using System.Windows;

namespace EmployeersManager.ViewModels;

public partial class PositionEditViewModel : ObservableObject
{
    private readonly IPositionRepository _positionRepository;

    [ObservableProperty]
    private Position _position;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private bool _isSaving;

    [ObservableProperty]
    private string _pageTitle;

    public PositionEditViewModel(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    } 
    
    public PositionEditViewModel(IPositionRepository positionRepository, Position position) : this(positionRepository)
    {
        Initialize(position);
    }

    public void Initialize(Position position)
    {
        Position = new Position
        {
            Id = position.Id,
            Name = position.Name,
            Enabled = position.Enabled,
            ColorHEX = position.ColorHEX
        };

        IsNew = position.Id == 0;
        PageTitle = IsNew ? "Додавання нової позиції" : "Редагування позиції";
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Position.Name))
        {
            MessageBox.Show("Назва позиції не може бути порожньою", "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(Position.ColorHEX) || !IsValidHexColor(Position.ColorHEX))
        {
            MessageBox.Show("Введіть правильний HEX-код кольору (наприклад, #FFFFFFFF)", "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        IsSaving = true;

        try
        {
            if (IsNew)
            {
                await _positionRepository.AddAsync(Position);
            }
            else
            {
                await _positionRepository.UpdateAsync(Position);
            }

            WeakReferenceMessenger.Default.Send(new NavigationMessage(ViewModelNavigation.Positions));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка збереження позиції: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsSaving = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        WeakReferenceMessenger.Default.Send(new NavigationMessage(ViewModelNavigation.Positions));
    }

    private static bool IsValidHexColor(string color)
    {
        if (string.IsNullOrEmpty(color)) return false;

        if (!color.StartsWith("#") || (color.Length != 7 && color.Length != 9)) return false;

        for (int i = 1; i < color.Length; i++)
        {
            if (!char.IsDigit(color[i]) && (color[i] < 'A' || color[i] > 'F') && (color[i] < 'a' || color[i] > 'f'))
            {
                return false;
            }
        }

        return true;
    }
}
