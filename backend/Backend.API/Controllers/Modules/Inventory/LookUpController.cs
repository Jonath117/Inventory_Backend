using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.Interfaces.IServices;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Inventory;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/lookups")]
public class LookUpController : ControllerBase
{
    private readonly ILookUpService _lookUpService;
    private readonly ICurrentCompanyProvider _companyProvider;
    
    public LookUpController(ILookUpService lookUpService, ICurrentCompanyProvider companyProvider)
    {
        _lookUpService = lookUpService;
        _companyProvider = companyProvider;
    }

    [HttpGet("lookup-products")]
    public async Task<IActionResult> GetProductsForDropdownAsync()
    {
        int companyId = _companyProvider.CompanyId;
        var products = await _lookUpService.GetProductsForDropdown(companyId);
        return Ok(products);
    }

    [HttpGet("lookup-warehouses")]
    public async Task<IActionResult> GetWarehouseForDropdownAsync()
    {
        int companyId = _companyProvider.CompanyId;
        var warehouses = await _lookUpService.GetWarehouseForDropdown(companyId);
        return Ok(warehouses);
    }
}