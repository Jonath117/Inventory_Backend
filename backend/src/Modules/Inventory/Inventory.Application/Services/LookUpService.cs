using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Application.Services;

public class LookUpService: ILookUpService
{
    private readonly ILookUpRepository _repository;
    
    public LookUpService(ILookUpRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductLookUpDto>> GetProductsForDropdown(int companyId)
    {
        var products = await _repository.GetProductsForDropdownAsync(companyId);
        
        return products.Select(p => new ProductLookUpDto(p.Id, p.Sku, p.Name)).ToList();
    }

    public async Task<IEnumerable<WarehouseLookUpDto>> GetWarehouseForDropdown(int companyId)
    {
        var warehouses = await _repository.GetWarehouseForDropdownAsync(companyId);
        
        return warehouses.Select(w => new WarehouseLookUpDto(w.Id, w.Name)).ToList();
    }
}