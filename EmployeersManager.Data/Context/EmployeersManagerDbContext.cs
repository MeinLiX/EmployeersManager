using EmployeersManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeersManager.Data.Context;

public class EmployeersManagerDbContext(DbContextOptions<EmployeersManagerDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Position> Positions { get; set; }
}
