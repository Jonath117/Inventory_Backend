using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;

namespace Inventory.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ICurrentCompanyProvider _currentCompanyProvider;

    public ProductController(IProductService service, ICurrentCompanyProvider companyProvider)
    {
        _service = service;
        _currentCompanyProvider = companyProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(string companyCen) 
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;
            var productList = await _service.GetProductsAsync(companyId);
            return Ok(productList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocurrió un error interno al listar los productos." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(string companyCen, [FromBody] CreateProductContractRequest request) 
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;
            var createdProduct = await _service.CreateProductAsync(companyId, request);
            return Created("", createdProduct);
        } 
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{productCen}")]
    public async Task<IActionResult> EditProduct(string companyCen, string productCen, [FromBody] UpdateProductContractRequest request) 
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;
            var updatedProduct = await _service.EditProductAsync(companyId, productCen, request);
            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("{productCen}/status")]
    public async Task<IActionResult> UpdateProductStatus(string companyCen, string productCen, [FromBody] UpdateProductStatusContractRequest request)
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;
            var updatedProduct = await _service.UpdateProductStatusAsync(companyId, productCen, request.Status);
            return Ok(updatedProduct); 
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("lookup")]
    public async Task<IActionResult> LookupProducts(string companyCen, [FromBody] ProductLookupContractRequest request) 
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;
            var products = await _service.LookupProductsAsync(companyId, request);
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("sellable")]
    public async Task<IActionResult> GetSellableProducts(
        string companyCen, 
        [FromQuery] string? search, 
        [FromQuery] string? categoryCen, 
        [FromQuery] string? warehouseCen, 
        [FromQuery] bool onlyAvailable = true, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 50)
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;
            var products = await _service.GetSellableProductsAsync(companyId, search, categoryCen, warehouseCen, onlyAvailable, page, pageSize);
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
    
    [HttpGet("{productCen}")]
    public async Task<IActionResult> GetProductDetails(string companyCen, string productCen) // <-- Agregado
    {
        try
        {
            int companyId = _currentCompanyProvider.CompanyId;
            var product = await _service.GetProductDetailsByCenAsync(companyId, productCen);
            
            if (product == null)
            {
                return NotFound(new { error = $"El producto con código {productCen} no existe." });
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocurrió un error interno en el servidor." });
        }
    }
}