using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeersManager.Core.Models;

public partial class Employee : ObservableObject
{
    [ObservableProperty]
    [Key]
    public int _id;

    [ObservableProperty]
    private string _fullName;

    [ObservableProperty]
    private decimal _salary;

    [ObservableProperty]
    private DateTime _hireDate;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(IsActive))]
    private DateTime? _terminationDate;

    public bool IsActive => TerminationDate == null;

    [ObservableProperty]
    public int _positionId;

    [ObservableProperty]
    private Position _position;
}
