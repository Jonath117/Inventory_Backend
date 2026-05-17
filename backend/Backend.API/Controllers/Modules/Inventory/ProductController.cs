using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Inventory;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/products")]
public class ProductController: ControllerBase
{
    private readonly IProductService _service;
    private readonly ICurrentCompanyProvider _currentCompanyProvider;

    public ProductController(IProductService service, ICurrentCompanyProvider companyProvider)
    {
        _service = service;
        _currentCompanyProvider = companyProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;

            var productList = await _service.GetProductsAsync(companyId);
            return Ok(productList);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            return StatusCode(500, new { error = "Ocurrió un error interno al listar los productos." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;

            var finalDto = dto with { CompanyId = companyId };
            
            var createdProduct = await _service.CreateProductAsync(finalDto);
            
            return Created("", createdProduct);
            
        } 
        catch (InvalidOperationException ex) 
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear producto: {ex}");
            return StatusCode(500, new { error = "Ocurrio un error interno en el servidor." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditProduct(int id, [FromBody] ProductUpdateDto dto)
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;

            var finalDto = dto with { CompanyId = companyId };
            var updatedProduct = await _service.EditProductAsync(id, finalDto);

            return Ok(updatedProduct);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message }); 
        }
        catch (InvalidOperationException ex) 
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocurrió un error interno." });
        }
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> DeactivateProduct(int id)
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;
            
            await _service.DesactiveProductAsync(companyId, id);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocurrio un error interno." });
        }
    }
    
    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> ActivateProduct(int id)
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;
            
            await _service.ActivateProductAsync(companyId, id);
            
            return NoContent();
            
        } 
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocurrio un error interno." });
        }
    }
}