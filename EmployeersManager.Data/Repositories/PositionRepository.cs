using EmployeersManager.Core.Interfaces;
using EmployeersManager.Core.Models;
using EmployeersManager.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeersManager.Data.Repositories;

public class PositionRepository(EmployeersManagerDbContext context) : IPositionRepository
{
    private readonly EmployeersManagerDbContext _context = context;

    //[skip] [take] for big data
    public async Task<IEnumerable<Position>> GetAllAsync()
    {
        return await _context.Positions.AsNoTracking().ToListAsync();
    }

    public async Task<Position> GetByIdAsync(int id)
    {
        return await _context.Positions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Position position)
    {
        await _context.Positions.AddAsync(position);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Position position)
    {
        var existing = await _context.Positions.FindAsync(position.Id);
        if (existing != null)
        {
            _context.Entry(existing).CurrentValues.SetValues(position);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var position = await _context.Positions.FindAsync(id);
        if (position is null) return false;

        _context.Positions.Remove(position);
        await _context.SaveChangesAsync();
        return true;
    }
}
