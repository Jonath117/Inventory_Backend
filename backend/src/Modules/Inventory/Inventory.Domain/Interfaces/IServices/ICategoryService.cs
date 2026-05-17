using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IServices;

public interface ICategoryService
{
    Task<IEnumerable<CategoryContractDto>> GetCategoriesAsync(int companyId);
    
    Task<CategoryContractDto> CreateCategoryAsync(CategoryCreateDto dto);
    
    Task UpdateCategoryAsync(CategoryUpdateDto dto);
}