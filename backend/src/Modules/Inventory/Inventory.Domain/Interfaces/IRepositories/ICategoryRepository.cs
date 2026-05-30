using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;


namespace Inventory.Domain.Interfaces.IRepositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategories(int companyId);
    
    Task AddAsync(Category category);
    
    Task UpdateAsync(Category category);
    
    Task<Category?> GetByCategoryCenAsync(string categoryCen, int companyId);
    Task<(int Id, string Name)> GetInfoByCenAsync(int companyId, string categoryCen);
}