using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;

namespace Inventory.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/documents")]
public class DocumentController : ControllerBase
{
    private readonly IMovementService _movementService;
    private readonly ICurrentCompanyProvider _companyProvider;

    public DocumentController(IMovementService movementService, ICurrentCompanyProvider companyProvider)
    {
        _movementService = movementService;
        _companyProvider = companyProvider;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDocument([FromBody] InventoryDocumentContractRequest request)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;
            var document = await _movementService.CreateDocumentAsync(companyId, request);
            return StatusCode(201, document);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetDocuments([FromQuery] string? documentType, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;
            var documents = await _movementService.GetDocumentsAsync(companyId, documentType, from, to);
            return Ok(documents);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}