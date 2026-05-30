using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<IEnumerable<CategoryContractDto>> GetCategoriesAsync(int companyId)
    {
        var categories = await _repository.GetCategories(companyId);
        return categories.Select(c => new CategoryContractDto(
            c.CategoryCen, 
            c.Name, 
            c.Description, 
            c.IsActive
        )).ToList();
        
    }

    public async Task<CategoryContractDto> CreateCategoryAsync(CategoryCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("El nombre de la categoria es obligatorio");
        
        var newCategory = new Category(dto.CompanyId, dto.Name, dto.Description);
        
        await _repository.AddAsync(newCategory);
        
        return new CategoryContractDto(
            newCategory.CategoryCen, 
            newCategory.Name, 
            newCategory.Description, 
            newCategory.IsActive
        );
    }

    public async Task UpdateCategoryAsync(CategoryUpdateDto dto)
    {
        var category = await _repository.GetByCategoryCenAsync(dto.CategoryCen, dto.CompanyId);

        if (category == null)
        {
            throw new KeyNotFoundException($"La categoría con código {dto.CategoryCen} no existe.");
        }
        
        category.Update(dto.Name, dto.Description);
        
        await _repository.UpdateAsync(category);
    }
}