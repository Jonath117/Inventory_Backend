using Microsoft.AspNetCore.Mvc;
using WebApp1.Domain.DTOs;
using WebApp1.Domain.Interfaces;

namespace WebApp1.Application;

[ApiController]
[Route("api/[controller]")]

public class AdjustmentController: Controller
{
    private readonly IAdjustmentService  _inventoryService;

    public AdjustmentController(IAdjustmentService inventoryService)
    {
        _inventoryService = inventoryService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAjustment(
        [FromHeader(Name = "x-company-id")] int companyId,
        [FromBody] AdjustmentDto request
    )
    {
        try
        {
            if (companyId <= 0) return BadRequest("Header COmpany Requerido");

            await _inventoryService.RegisterAdjustmentAsync(companyId, request);

            return Ok(new { message = "Ajuste registrado correctamente. Ajuste realizado" });
        }
        catch (Exception ex)
        {
            return BadRequest(new {error = ex.Message});
        }
    }
}