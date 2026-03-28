using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers.Modules.Inventory;

[ApiController]
[Route("api/[controller]")]
public class UnitController(IUnitService uniteService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUnits([FromHeader(Name = "x-company-id")]int companyId)
    {
        try
        {
            if (companyId <= 0) return BadRequest("Company id invalido");

            var units = await uniteService.GetUnitsAsync(companyId);
            return Ok(units);
            
        } catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocurrio un error interno en el servidor." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatUnit([FromHeader(Name = "x-company-id")] int companyId,
        [FromBody] UnitCreateDto dto)
    {
        try
        {
            if (companyId <= 0) return BadRequest("Company id invalido");

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