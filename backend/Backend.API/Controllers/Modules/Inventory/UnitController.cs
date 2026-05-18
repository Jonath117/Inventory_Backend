using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Inventory;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/units")]
public class UnitController(IUnitService _unitService, ICurrentCompanyProvider _companyProvider) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetUnits()
    {
        try
        {
            int companyId = _companyProvider.CompanyId;

            var units = await _unitService.GetUnitsAsync(companyId);
            return Ok(units);
            
        } catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocurrio un error interno en el servidor." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUnit([FromBody] CreateUnitContractRequest request)
    {
        int companyId = _companyProvider.CompanyId;
        var createdUnit = await _unitService.CreateUnitAsync(companyId, request);
        return StatusCode(201, createdUnit);
    }
    
    [HttpPut("{unitCen}")]
    public async Task<IActionResult> UpdateUnit(string unitCen, [FromBody] CreateUnitContractRequest request)
    {
        int companyId = _companyProvider.CompanyId;
        var updatedUnit = await _unitService.UpdateUnitAsync(companyId, unitCen, request);
        return Ok(updatedUnit);
    }
    

}