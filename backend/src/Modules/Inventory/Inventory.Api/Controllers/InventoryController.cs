using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.Interfaces.IServices;
using Shared.Application.Interfaces;

namespace Inventory.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly ICurrentCompanyProvider _companyProvider;

    public InventoryController(IInventoryService inventoryService, ICurrentCompanyProvider companyProvider)
    {
        _inventoryService = inventoryService;
        _companyProvider = companyProvider;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        int companyId = _companyProvider.CompanyId;
        var dashboard = await _inventoryService.GetDashboardMetricsAsync(companyId);
        return Ok(dashboard);
    }

    [HttpGet("restock-events")]
    public async Task StreamRestockEvents([FromServices] System.Threading.Channels.Channel<Inventory.Domain.DTOs.RestockEvent> restockChannel, CancellationToken ct)
    {
        Response.Headers["Content-Type"] = "text/event-stream";
        Response.Headers["Cache-Control"] = "no-cache";

        await foreach (var evento in restockChannel.Reader.ReadAllAsync(ct))
        {
            await Response.WriteAsync($"data: {System.Text.Json.JsonSerializer.Serialize(evento)}\n\n");
            await Response.Body.FlushAsync(ct);
        }
    }
}