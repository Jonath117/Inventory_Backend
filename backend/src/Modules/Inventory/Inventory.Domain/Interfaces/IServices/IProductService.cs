using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IProductService
{
    Task<IEnumerable<ProductContractDto>> GetProductsAsync(int companyId);
    Task<ProductContractDto> CreateProductAsync(int companyId, CreateProductContractRequest request);
    Task<ProductContractDto> EditProductAsync(int companyId, string productCen, UpdateProductContractRequest request);
    Task<ProductContractDto> UpdateProductStatusAsync(int companyId, string productCen, string status);
}