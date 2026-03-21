using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;


namespace Inventory.Domain.Interfaces.IRepositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategories(int companyId);
    
    Task AddAsync(Category category);
    
    Task UpdateAsync(Category category);
    
    Task<Category?> GetByIdAsync(int id, int companyId);
    
}