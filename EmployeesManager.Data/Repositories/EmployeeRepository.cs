using EmployeesManager.Core.AggregationModels;
using EmployeesManager.Core.Interfaces.Repository;
using EmployeesManager.Core.Models;
using EmployeesManager.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Data.Repositories;

public class EmployeeRepository(EmployeesManagerDbContext context) : IEmployeeRepository
{
    private readonly EmployeesManagerDbContext _context = context;

    //[skip] [take] for big data
    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees.Include(e => e.Position).AsNoTracking().ToListAsync();
    }

    public async Task<Employee> GetByIdAsync(int id)
    {
        return await _context.Employees.Include(e => e.Position)
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Employee>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return [];

        string lowerSearchTerm = searchTerm.ToLowerInvariant();
        bool isIdSearch = int.TryParse(searchTerm, out int searchId);

        var query = _context.Employees.Include(e => e.Position);

        IQueryable<Employee> finalQuery;
        if (isIdSearch)
            finalQuery = query.Where(e => e.Id == searchId);
        else
            finalQuery = query.Where(e => e.FullName.ToLower().Contains(lowerSearchTerm.ToLower()));

        return await finalQuery.AsNoTracking().ToListAsync();
    }

    public Task AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        return _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        var existingEmployee = await _context.Employees
                                             .Include(e => e.Position)
                                             .FirstOrDefaultAsync(e => e.Id == employee.Id);

        if (existingEmployee != null)
        {
            _context.Entry(existingEmployee).CurrentValues.SetValues(employee);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> TerminateAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return false;

        employee.TerminationDate = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<EmployeeStatistics> GetStatisticsAsync()
    {
        var allEmployees = _context.Employees;

        var totalCountTask = allEmployees.CountAsync();

        var activeEmployeesQuery = allEmployees.Where(e => e.TerminationDate == null);
        var activeCountTask = activeEmployeesQuery.CountAsync();


        var averageSalaryTask = activeEmployeesQuery.AverageAsync(e => (decimal?)e.Salary);

        var today = DateTime.UtcNow;

        //SQLITE not supp EF.Functions.DateDiffDay
        var employeeDatesTask = allEmployees
            .Select(e => new { e.HireDate, e.TerminationDate })
            .ToListAsync();


        var positionsCountTask = allEmployees
            .Where(e => e.Position != null && !string.IsNullOrEmpty(e.Position.Name))
            .Include(e => e.Position)
            .GroupBy(e => e.Position.Name)
            .Select(g => new { PositionName = g.Key, Count = g.Count() })
            .ToDictionaryAsync(
                result => result.PositionName,
                result => result.Count
            );

        await Task.WhenAll(
            totalCountTask,
            activeCountTask,
            averageSalaryTask,
            employeeDatesTask,
            positionsCountTask
        );

        int totalEmployees = await totalCountTask;
        int activeEmployees = await activeCountTask;
        int terminatedEmployees = totalEmployees - activeEmployees;

        var employeeDates = await employeeDatesTask;
        double averageDaysWorked = 0.0;

        if (employeeDates.Count != 0) 
        {
            averageDaysWorked = employeeDates.Average(e =>
            {
                DateTime endDate = e.TerminationDate ?? today;
                return (endDate - e.HireDate).TotalDays;
            });
        }

        var stats = new EmployeeStatistics(
            TotalEmployees: totalEmployees,
            ActiveEmployees: activeEmployees,
            TerminatedEmployees: terminatedEmployees,
            AverageSalary: (await averageSalaryTask) ?? 0m,
            AverageDaysWorked: averageDaysWorked,
            PositionsCount: await positionsCountTask
        );

        return stats;
    }

    public async Task ImportRangeAsync(IEnumerable<Employee> employees)
    {
        //Here can validate same Id, etc ...

        foreach (var employee in employees)
        {
            if (employee.Position != null)
            {
                var existingPosition = await _context.Positions
                    .FindAsync(employee.Position.Id);

                employee.Position = existingPosition ?? null;
            }

            _context.Employees.Add(employee);
        }
        await _context.SaveChangesAsync();
    }
}
