using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.Interfaces;
using Inventory.Domain.Interfaces.IServices;

namespace Backend.API.Modules.Inventory;

[ApiController]
[Route("api/[controller]")]
public class KardexController : ControllerBase
{
    private readonly IGetProductKardexService _productKardexService;

    public KardexController(IGetProductKardexService productKardexService)
    {
        _productKardexService = productKardexService;
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetKardex(
        [FromHeader(Name = "x-company-id")] int companyId,
        int productId,
        [FromQuery] int? warehouseId
    )
    {
        try
        {
            if (companyId <= 0) return BadRequest("Header de x-company-id requerido");
            if (productId <= 0) return BadRequest("Id de producto nov valido");

            var history = await _productKardexService.GetProductKardexAsync(companyId, productId, warehouseId);

            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}