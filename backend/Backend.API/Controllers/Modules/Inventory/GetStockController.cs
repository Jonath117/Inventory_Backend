using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.Interfaces;
using Inventory.Domain.Interfaces.IServices;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Inventory;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/stock")]
public class GetStockController : ControllerBase
{
    private readonly IGetStockService _getStockService;
    private readonly ICurrentCompanyProvider _companyProvider;

    public GetStockController(IGetStockService getStockService, ICurrentCompanyProvider companyProvider)
    {
        _getStockService = getStockService;
        _companyProvider = companyProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetStock([FromQuery] int? warehouseId)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;
            
            var stock = await _getStockService.GetCurrentStockAsync(companyId, warehouseId);
            
            return Ok(stock);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}