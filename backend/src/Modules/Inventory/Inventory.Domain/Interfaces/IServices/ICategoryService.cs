using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IServices;

public interface ICategoryService
{
    Task<IEnumerable<CategoryLookupDto>> GetCategoriesAsync(int companyId);
    
    Task CreateCategoryAsync(CategoryCreateDto dto);
    
    Task UpdateCategoryAsync(CategoryUpdateDto dto);
}