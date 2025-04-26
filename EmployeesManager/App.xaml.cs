using EmployeesManager.Core.Enums;
using EmployeesManager.Core.Interfaces;
using EmployeesManager.Core.Interfaces.Repository;
using EmployeesManager.Core.Interfaces.Services;
using EmployeesManager.Core.Models;
using EmployeesManager.Data.Context;
using EmployeesManager.Data.Repositories;
using EmployeesManager.Dialog;
using EmployeesManager.Infrastructure.Services;
using EmployeesManager.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace EmployeesManager;

public partial class App : Application
{
    internal new static App Current => (App)Application.Current;

    internal IServiceProvider ServicesProvider { get; }

    public App()
    {
        ServiceCollection services = new();
        ConfigureServices(services);
        ServicesProvider = services.BuildServiceProvider();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddLogging();

        //Todo modal window with select db path and save it to appSettings
        services.AddDbContext<EmployeesManagerDbContext>(options =>
            options.UseSqlite("Data Source=employees.db"));
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        services.AddTransient<IPositionRepository, PositionRepository>();

        services.AddTransient<IDataImportService, DataImportService>();
        services.AddTransient<IDataExportService, DataExportService>();

        services.AddSingleton<IDialogService, DialogService>();

        services.AddTransient<MainViewModel>();
        services.TryAddKeyedTransient<IDialogView, ExportImportDialog>(DialogViews.ExportImportDialog);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        if (!IsDotNet9RuntimeInstalled())
        {
            MessageBox.Show(
                ".NET 9 Runtime не встановлений. Ви будете перенаправлені на офіційну сторінку завантаження.",
                "Необхідний .NET 9",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );

            Process.Start(new ProcessStartInfo
            {
                FileName = "https://dotnet.microsoft.com/en-us/download/dotnet/9.0/runtime",
                UseShellExecute = true
            });

            Current.Shutdown();
        }

        DatabaseInitializer();

        base.OnStartup(e);
    }


    private bool IsDotNet9RuntimeInstalled()
    {
        var runtimePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "dotnet",
            "shared",
            "Microsoft.WindowsDesktop.App"
        );

        if (!Directory.Exists(runtimePath))
            return false;

        var versions = Directory.GetDirectories(runtimePath)
                                .Select(Path.GetFileName)
                                .Where(name => name.StartsWith("9.0"))
                                .ToList();

        return versions.Any();
    }

    private void DatabaseInitializer()
    {
        // like DatabaseInitializer Service
        var db = ServicesProvider.GetService<EmployeesManagerDbContext>();
        if (db is not null && db.Database.CanConnect())
        {
            // TODO check exist db and do not recreate it. For employeers save u can use import/export JSON
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();


            //Default positions
            db.Positions.AddRange(
            [
                new Position { Name = "Manager", ColorHEX = "#FF6B6B", Enabled = true },
                new Position { Name = "Developer", ColorHEX = "#4ECDC4", Enabled = true },
                new Position { Name = "Designer" , ColorHEX = "#FFD93D", Enabled = true },
                new Position { Name = "QA", ColorHEX = "#1A535C", Enabled = true }

            ]);
            db.SaveChanges();
        }
    }
}
