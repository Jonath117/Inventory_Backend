using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IProductService
{
    Task<IEnumerable<ProductContractDto>> GetProductsAsync(int companyId);
    Task<ProductContractDto> CreateProductAsync(int companyId, CreateProductContractRequest request);
    Task<ProductContractDto> EditProductAsync(int companyId, string productCen, UpdateProductContractRequest request);
    Task<ProductContractDto> UpdateProductStatusAsync(int companyId, string productCen, string status);
    Task<IEnumerable<ProductContractDto>> LookupProductsAsync(int companyId, ProductLookupContractRequest request);
    Task<IEnumerable<SellableProductContractDto>> GetSellableProductsAsync(int companyId, string? search, string? categoryCen, string? warehouseCen, bool onlyAvailable, int page, int pageSize);
}