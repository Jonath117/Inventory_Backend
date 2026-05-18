using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class UnitRepository : IUnitRepository
{
    private readonly InventoryDbContext _context;

    public UnitRepository(InventoryDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Unit>> GetUnitsAsync(int companyId)
    {
        return await _context.Units
            .Where(u => u.CompanyId == companyId)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameAsync(int companyId, string name)
    {
        return await _context.Units
            .AnyAsync(u => u.CompanyId == companyId && u.Name.ToLower() == name.ToLower());
    }

    public async Task<Unit> AddAsync(Unit unit)
    {
        await _context.Units.AddAsync(unit);
        await _context.SaveChangesAsync();
        return unit;
    }

    public async Task<(int Id, string Name)> GetInfoByCenAsync(int companyId, string unitCen)
    {
        var unit = await _context.Units
            .AsNoTracking()
            .Where(u => u.CompanyId == companyId && u.UnitCen == unitCen)
            .Select(u => new { u.Id, u.Name })
            .FirstOrDefaultAsync();

        if (unit == null) return (0, string.Empty);
        return (unit.Id, unit.Name);
    }
    
    public async Task<Unit?> GetByUnitCenAsync(int companyId, string unitCen)
    {
        return await _context.Units
            .FirstOrDefaultAsync(u => u.CompanyId == companyId && u.UnitCen == unitCen);
    }
    
    public async Task UpdateAsync(Unit unit)
    {
        _context.Units.Update(unit);
        await _context.SaveChangesAsync();
    }
}