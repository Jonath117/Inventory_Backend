using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers.Modules.Inventory;

[ApiController]
[Route("api/[controller]")]
public class ProductController: ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromHeader(Name = "x-company-id")] int companyId)
    {
        try
        {
            if (companyId <= 0)
            {
                return BadRequest(new { error = "Id de compañia Invalido" });
            }

            var productList = await _service.GetProductsAsync(companyId);
            return Ok(productList);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            return StatusCode(500, new { error = "Ocurrió un error interno." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromHeader(Name = "x-company-id")] int companyId,
        [FromBody] ProductCreateDto dto)
    {
        try
        {
            if (companyId <= 0)
            {
                return BadRequest(new { error = "Id de compañia Invalido" });
            }

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
    public async Task<IActionResult> EditProduct(int id, [FromHeader(Name = "x-company-id")] int companyId,
        [FromBody] ProductUpdateDto dto)
    {
        try
        {
            if (companyId <= 0) return BadRequest(new { error = "Company Id invalido" });

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
    public async Task<IActionResult> DeactivateProduct(int id, [FromHeader(Name = "x-company-id")] int companyId)
    {
        try
        {
            if (companyId <= 0) return BadRequest(new { error = "Company Id invalido" });
            
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
    public async Task<IActionResult> ActivateProduct(int id, [FromHeader(Name = "x-company-id")] int companyId)
    {
        try
        {
            if (companyId <= 0) return BadRequest(new { error = "Company Id invalido" });
            
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