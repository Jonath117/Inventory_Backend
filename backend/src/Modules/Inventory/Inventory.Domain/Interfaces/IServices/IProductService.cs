using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(int companyId);
    Task<ProductLookUpDto> CreateProductAsync(ProductCreateDto dto);
    Task<ProductLookUpDto> EditProductAsync(int productId, ProductUpdateDto dto);
    Task DesactiveProductAsync(int companyId, int productId);
    Task ActivateProductAsync(int companyId, int productId);
}