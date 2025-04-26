using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EmployeersManager.Core;
using EmployeersManager.Core.Enums;
using EmployeersManager.Core.Interfaces;
using EmployeersManager.Core.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace EmployeersManager.ViewModels;

public partial class PositionsViewModel : ObservableObject
{
    private readonly IPositionRepository _positionRepository;

    public PositionsViewModel(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
        LoadPositionsCommand.Execute(null);
    }

    [ObservableProperty]
    private ObservableCollection<Position> _positions = new();

    [ObservableProperty]
    private Position _selectedPosition;

    [ObservableProperty]
    private bool _isLoading;

    [RelayCommand]
    private async Task LoadPositions()
    {
        IsLoading = true;

        try
        {
            var positions = await _positionRepository.GetAllAsync();
            Positions = [.. positions];
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка завантаження позицій: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void AddPosition()
    {
        WeakReferenceMessenger.Default.Send(new NavigationMessage<Position>(ViewModelNavigation.PositionEdit, new Position { Enabled = true, ColorHEX = "#FFFFFFFF"}));
    }

    [RelayCommand]
    private void EditPosition()
    {
        if (SelectedPosition != null)
        {
            WeakReferenceMessenger.Default.Send(new NavigationMessage<Position>(ViewModelNavigation.PositionEdit, SelectedPosition));
        }
    }

    [RelayCommand]
    private async Task DeletePosition()
    {
        if (SelectedPosition == null) return;

        var result = MessageBox.Show($"Ви впевнені, що хочете видалити позицію '{SelectedPosition.Name}'?",
            "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                await _positionRepository.DeleteAsync(SelectedPosition.Id);
                Positions.Remove(SelectedPosition);
                SelectedPosition = null;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Помилка видалення позиції: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
