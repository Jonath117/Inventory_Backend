using Microsoft.AspNetCore.Mvc;
using WebApp1.Domain.Interfaces;

namespace WebApp1.Controllers;

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
            if (companies.Count == 0)
            {
                return NoContent();
            }
            return Ok(companies);
        }  
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}