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

    public async Task<IEnumerable<ProductLookUpDto>> GetProductsForDropdownAsync(int companyId)
    {
        return await _repository.GetProductsForDropdownAsync(companyId);
    }

    public async Task<IEnumerable<WarehouseLookUpDto>> GetWarehouseForDropdownAsync(int companyId)
    {
        return await _repository.GetWarehouseForDropdownAsync(companyId);
    }
}