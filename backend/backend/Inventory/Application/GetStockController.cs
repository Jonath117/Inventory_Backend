using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApp1.Domain.Interfaces;

namespace WebApp1.Application;

[ApiController]
[Route("api/[controller]")]
public class GetStockController : ControllerBase
{
    private readonly IGetStockService _getStockService;

    public GetStockController(IGetStockService getStockService)
    {
        _getStockService = getStockService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStock(
        [FromHeader(Name = "x-company-id")] int companyId,
        [FromQuery] int? warehouseId
    )
    {
        try
        {
            if (companyId <= 0) return BadRequest("header requerido");
            
            var stock = await _getStockService.GetCurrentStockAsync(companyId, warehouseId);
            
            return Ok(stock);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}