using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Inventory;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/units")]
public class UnitController(IUnitService uniteService, ICurrentCompanyProvider companyProvider) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetUnits()
    {
        try
        {
            int companyId = companyProvider.CompanyId;

            var units = await uniteService.GetUnitsAsync(companyId);
            return Ok(units);
            
        } catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocurrio un error interno en el servidor." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatUnit([FromBody] UnitCreateDto dto)
    {
        try
        {
            int companyId = companyProvider.CompanyId;

            var finalDto = dto with { CompanyId = companyId };
            var createdUnit = await uniteService.CreateUnitAsync(finalDto);

            return Created("", createdUnit);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocurrio un error interno en el servidor." });
        }
    }
    

}