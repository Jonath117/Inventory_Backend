using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Inventory;

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
        try
        {
            int companyId = _companyProvider.CompanyId;
            var categories = await _categoryService.GetCategoriesAsync(companyId);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrio un error {ex}");
            return StatusCode(500, new { error = "Ocurrió un error interno en el servidor." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryContractRequest request)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;
            
            var finalDto = new CategoryCreateDto(companyId, request.Name, request.Description);
            
            var createdCategory = await _categoryService.CreateCategoryAsync(finalDto);
            
            return StatusCode(201, createdCategory);
        }
        catch(Exception ex)
        {
            return BadRequest(new {error = ex.Message});
        }
    }
    
    [HttpPut("{categoryCen}")]
    public async Task<IActionResult> UpdateCategory(string categoryCen, [FromBody] CreateCategoryContractRequest request)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;
            var finalDto = new CategoryUpdateDto(categoryCen, companyId, request.Name, request.Description);
            
            await _categoryService.UpdateCategoryAsync(finalDto);
            
            return Ok(new { categoryCen = categoryCen, name = request.Name, description = request.Description, isActive = true });
        }
        catch (Exception e)
        {
            return BadRequest(new {error = e.Message});
        }
    }
}