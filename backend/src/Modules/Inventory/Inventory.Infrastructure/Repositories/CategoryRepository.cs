using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class CategoryRepository:  ICategoryRepository
{
    private readonly InventoryDbContext _context;

    public CategoryRepository(InventoryDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Category>> GetCategories(int companyId)
    {
        return await _context.Categories
            .Where(c => c.CompanyId == companyId)
            .ToListAsync();
        
    }

    public async Task<Category?> GetByIdAsync(int id, int companyId)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.CompanyId == companyId);
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }
}