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
    
    
    public async Task<IEnumerable<CategoryLookupDto>> GetCategoriesAsync(int companyId)
    {
        var categories = await _repository.GetCategories(companyId);
        return categories.Select(c => new CategoryLookupDto(c.Id, c.Name, c.Description)).ToList();
        
    }

    public async Task CreateCategoryAsync(CategoryCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("El nommbre de la categoria es obligatorio");
        }

        var newCategory = new Category
        {
            CompanyId = dto.CompanyId,
            Name = dto.Name,
            Description = dto.Description,
        };
        
        await _repository.AddAsync(newCategory);
    }

    public async Task UpdateCategoryAsync(CategoryUpdateDto dto)
    {
        var category = await _repository.GetByIdAsync(dto.Id, dto.CompanyId);

        if (category == null)
        {
            throw new ArgumentException("Categoria no encontrada");
        }
        
        category.Name = dto.Name;
        category.Description = dto.Description;
        
        await _repository.UpdateAsync(category);
    }
}