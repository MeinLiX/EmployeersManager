using EmployeersManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace EmployeersManager;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Current.ServicesProvider.GetRequiredService<MainViewModel>();
    }
}