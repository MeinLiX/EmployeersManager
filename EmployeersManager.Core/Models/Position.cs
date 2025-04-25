using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeersManager.Core.Models;

public partial class Position : ObservableObject
{
    [ObservableProperty]
    [Key]
    public int _id;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private bool _enabled;

    [ObservableProperty]
    private string _colorHEX;

    public virtual ICollection<Employee> Employees { get; set; } = [];
}
