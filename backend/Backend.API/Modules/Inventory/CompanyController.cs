using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.Interfaces.IServices;

namespace Backend.API.Modules.Inventory;

[ApiController]
[Route("api/[controller]")]
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
            if (companies.Count == null || !companies.Any())
            {
                return NoContent();
            }
            return Ok(companies);
        }  
        catch (Exception ex)
        {
            Console.WriteLine($"error: {ex.ToString()}");
            return StatusCode(500, new { error = ex.Message });
        }
    }

}