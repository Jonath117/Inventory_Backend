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
    

}