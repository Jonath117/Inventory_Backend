using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.Interfaces;
using Inventory.Domain.Interfaces.IServices;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Inventory;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/products/{productCen}/kardex")]
public class KardexController : ControllerBase
{
    private readonly IGetProductKardexService _productKardexService;
    private readonly ICurrentCompanyProvider _companyProvider;

    public KardexController(IGetProductKardexService productKardexService, ICurrentCompanyProvider companyProvider)
    {
        _productKardexService = productKardexService;
        _companyProvider = companyProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetKardex(string productCen, [FromQuery] string? warehouseCen, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;

            var history = await _productKardexService.GetProductKardexAsync(companyId, productCen, warehouseCen, from, to);

            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}