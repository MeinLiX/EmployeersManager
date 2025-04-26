using EmployeesManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Data.Context;

public class EmployeesManagerDbContext(DbContextOptions<EmployeesManagerDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Position> Positions { get; set; }
}
