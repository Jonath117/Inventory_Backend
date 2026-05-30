using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Inventory;

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
    public async Task<IActionResult> GetProducts()
    {
        int companyId = _currentCompanyProvider.CompanyId;
        var productList = await _service.GetProductsAsync(companyId);
        return Ok(productList);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductContractRequest request)
    {
        int companyId = _currentCompanyProvider.CompanyId;
        var createdProduct = await _service.CreateProductAsync(companyId, request);
        return Created("", createdProduct);
    }

    [HttpPut("{productCen}")]
    public async Task<IActionResult> EditProduct(string productCen, [FromBody] UpdateProductContractRequest request)
    {
        int companyId = _currentCompanyProvider.CompanyId;
        var updatedProduct = await _service.EditProductAsync(companyId, productCen, request);
        return Ok(updatedProduct);
    }

    [HttpPatch("{productCen}/status")]
    public async Task<IActionResult> UpdateProductStatus(string productCen, [FromBody] UpdateProductStatusContractRequest request)
    {
        int companyId = _currentCompanyProvider.CompanyId;
        var updatedProduct = await _service.UpdateProductStatusAsync(companyId, productCen, request.Status);
        return Ok(updatedProduct); 
    }

    [HttpPost("lookup")]
    public async Task<IActionResult> LookupProducts([FromBody] ProductLookupContractRequest request)
    {
        int companyId = _currentCompanyProvider.CompanyId;
        var products = await _service.LookupProductsAsync(companyId, request);
        return Ok(products);
    }

    [HttpGet("~/api/inventory/companies/{companyCen}/sellable-products")]
    public async Task<IActionResult> GetSellableProducts(
        [FromQuery] string? search, 
        [FromQuery] string? categoryCen, 
        [FromQuery] string? warehouseCen, 
        [FromQuery] bool onlyAvailable = true, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 50)
    {
        int companyId = _currentCompanyProvider.CompanyId;
        var products = await _service.GetSellableProductsAsync(companyId, search, categoryCen, warehouseCen, onlyAvailable, page, pageSize);
        return Ok(products);
    }
}