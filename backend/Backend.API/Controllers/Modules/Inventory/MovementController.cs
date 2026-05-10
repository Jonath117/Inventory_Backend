using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces;
using Inventory.Domain.Interfaces.IServices;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Inventory;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/documents")]
public class MovementController : ControllerBase
{
    private readonly IMovementService _movementService;
    private readonly ICurrentCompanyProvider _companyProvider;

    public MovementController(IMovementService movementService, ICurrentCompanyProvider companyProvider)
    {
        _movementService = movementService;
        _companyProvider = companyProvider;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterMovement([FromBody] MovementDto request)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;

            await _movementService.RegisterMovement(companyId, request);

            string accion = request.MovementType == "IN" ? "Ingreso" : "Salida";
            return Ok(new { message = $"{accion} registrada correctamente" });

        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
}