using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Modules.Inventory;

[ApiController]
[Route("api/[controller]")]
public class CategoryController: ControllerBase
{
    private readonly ICategoryService _categoryService;
    
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }


    [HttpGet]
    public async Task<IActionResult> GetCategories([FromHeader(Name = "x-company-id")] int companyId)
    {
        try
        {
            if (companyId <= 0)
            {
                return BadRequest("Company Id invalido");
            }
            
            var categories = await _categoryService.GetCategoryAsync(companyId);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrio un error {ex}");
            return StatusCode(500, new { error = "Ocurrió un error interno en el servidor." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromHeader(Name = "x-company-id")] int companyId,
        [FromBody] CategoryCreateDto dto)
    {
        try
        {
            if (companyId <= 0)
            {
                return BadRequest(new {error = "Company Id invalido"});
            }
            
            var finalDto = dto with {CompanyId = companyId};
            
            await _categoryService.CreateCategoryAsync(finalDto);
            
            return Ok(new {message = "Categoria creada correctamente"});
            
        }
        catch(Exception ex)
        {
            return BadRequest(new {error = ex.Message});
        }
    }
}