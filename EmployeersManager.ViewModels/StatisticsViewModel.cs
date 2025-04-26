using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeersManager.Core.Interfaces;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace EmployeersManager.ViewModels;

public partial class StatisticsViewModel : ObservableObject
{
    private readonly IEmployeeRepository _repository;
    private readonly IPositionRepository _positionsRepository;

    [ObservableProperty]
    private int _totalEmployees;

    [ObservableProperty]
    private int _activeEmployees;

    [ObservableProperty]
    private int _terminatedEmployees;

    [ObservableProperty]
    private decimal _averageSalary;

    [ObservableProperty]
    private double _averageDaysWorked;


    [ObservableProperty]
    private SeriesCollection _employeePositionsSeries;


    public StatisticsViewModel(IEmployeeRepository employeeRepository, IPositionRepository positionRepository)
    {
        _repository = employeeRepository;
        _positionsRepository = positionRepository;
        LoadStatisticsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadStatisticsAsync()
    {
        var statsTask = _repository.GetStatisticsAsync();
        var positionsTask = _positionsRepository.GetAllAsync();


        await Task.WhenAll(
            statsTask,
            positionsTask
        );
        var stats = await statsTask;
        var positions = await positionsTask;

        TotalEmployees = stats.TotalEmployees;
        ActiveEmployees = stats.ActiveEmployees;
        TerminatedEmployees = stats.TerminatedEmployees;
        AverageSalary = stats.AverageSalary;
        AverageDaysWorked = stats.AverageDaysWorked;
        EmployeePositionsSeries =
            [..
            stats.PositionsCount.Select(p => new PieSeries{
                    Title = p.Key,
                    Values = new ChartValues<ObservableValue> { new(p.Value) },
                    DataLabels = true,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(positions.First(pos=> 0 == string.Compare(pos.Name, p.Key, StringComparison.Ordinal)).ColorHEX))
            })
            ];

    }
}