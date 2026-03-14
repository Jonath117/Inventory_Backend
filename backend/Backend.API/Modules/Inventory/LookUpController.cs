using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.Interfaces;
using Inventory.Domain.Interfaces.IServices;

namespace Backend.API.Modules.Inventory;

[ApiController]
[Route("api/[controller]")]
public class LookUpController : ControllerBase
{
    private readonly ILookUpService _lookUpService;
    
    public LookUpController(ILookUpService lookUpService)
    {
        _lookUpService = lookUpService;
    }

    [HttpGet("lookup-products")]
    public async Task<IActionResult> GetProductsForDropdownAsync([FromHeader(Name = "x-company-id")] int companyId)
    {
        if (companyId <= 0) return BadRequest("Id de compania invalido");
        var products = await _lookUpService.GetProductsForDropdownAsync(companyId);
        return Ok(products);
    }

    [HttpGet("lookup-warehouses")]
    public async Task<IActionResult> GetWarehouseForDropdownAsync([FromHeader(Name = "x-company-id")] int companyId)
    {
        if(companyId <= 0) return BadRequest("Id de la compañia invalido");
        var warehouses = await _lookUpService.GetWarehouseForDropdownAsync(companyId);
        return Ok(warehouses);
    }
}