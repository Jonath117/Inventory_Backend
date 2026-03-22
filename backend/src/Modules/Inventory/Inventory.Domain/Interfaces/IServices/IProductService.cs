using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(int companyId);
}