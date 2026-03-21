using Shared.Domain;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly InventoryDbContext _context;

    public CompanyRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Company>> GetActiveCompaniesAsync()
    {
        return await _context.Companies
            .Where(c => c.IsActive)
            .ToListAsync();
    }
}