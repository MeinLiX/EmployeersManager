using EmployeesManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace EmployeesManager;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Current.ServicesProvider.GetRequiredService<MainViewModel>();
    }
}