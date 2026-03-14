using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces;
using Inventory.Domain.Interfaces.IServices;

namespace Backend.API.Modules.Inventory;

[ApiController]
[Route("api/[controller]")]
public class MovementController : ControllerBase
{
    private readonly IMovementService _movementService;

    public MovementController(IMovementService movementService)
    {
        _movementService = movementService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterMovement(
        [FromHeader(Name = "x-company-id")] int companyId,
        [FromBody] MovementDto request
    )
    {
        try
        {
            if (companyId <= 0) return BadRequest("Header x-company-id requerido");

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