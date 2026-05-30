using Backend.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies")]
[AllowAnonymousTenant]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _inventoryService;

    public CompanyController(ICompanyService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveCompanies()
    {
        var companies = await _inventoryService.GetCompanyAsync();
        return Ok(companies);
    }

    [HttpGet("{companyCen}")]
    public async Task<IActionResult> GetCompanyByCen(string companyCen)
    {
        var company = await _inventoryService.GetCompanyByCenAsync(companyCen);
        return Ok(company);
    }
}