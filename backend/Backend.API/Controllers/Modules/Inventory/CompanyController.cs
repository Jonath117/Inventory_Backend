using Backend.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.Interfaces.IServices;

namespace Backend.API.Controllers.Modules.Inventory;

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
        try
        {
            var companies = await _inventoryService.GetCompanyAsync();
            return Ok(companies);
        }  
        catch (Exception ex)
        {
            Console.WriteLine($"error: {ex.ToString()}");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("{companyCen}")]
    public async Task<IActionResult> GetCompanyByCen(string companyCen)
    {
        try
        {
            var company = await _inventoryService.GetCompanyByCenAsync(companyCen);
            return Ok(company);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

}