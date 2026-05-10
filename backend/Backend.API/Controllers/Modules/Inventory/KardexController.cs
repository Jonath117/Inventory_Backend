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

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetKardex(int productId, [FromQuery] int? warehouseId)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;
            if (productId <= 0) return BadRequest("Id de producto nov valido");
            //int productId = _productProvider.productId

            var history = await _productKardexService.GetProductKardexAsync(companyId, productId, warehouseId);

            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}