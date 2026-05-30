using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;

namespace Inventory.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ICurrentCompanyProvider _companyProvider;
    
    public CategoryController(ICategoryService categoryService, ICurrentCompanyProvider companyProvider)
    {
        _categoryService = categoryService;
        _companyProvider = companyProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        int companyId = _companyProvider.CompanyId;
        var categories = await _categoryService.GetCategoriesAsync(companyId);
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryContractRequest request)
    {
        int companyId = _companyProvider.CompanyId;
        var finalDto = new CategoryCreateDto(companyId, request.Name, request.Description);
        var createdCategory = await _categoryService.CreateCategoryAsync(finalDto);
        return StatusCode(201, createdCategory);
    }
    
    [HttpPut("{categoryCen}")]
    public async Task<IActionResult> UpdateCategory(string categoryCen, [FromBody] CreateCategoryContractRequest request)
    {
        int companyId = _companyProvider.CompanyId;
        var finalDto = new CategoryUpdateDto(categoryCen, companyId, request.Name, request.Description);
        await _categoryService.UpdateCategoryAsync(finalDto);
        return Ok(new { categoryCen = categoryCen, name = request.Name, description = request.Description, isActive = true });
    }
}