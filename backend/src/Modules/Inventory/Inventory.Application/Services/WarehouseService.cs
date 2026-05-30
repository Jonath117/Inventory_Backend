using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Application.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _repository;

    public WarehouseService(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<WarehouseContractDto>> GetWarehousesAsync(int companyId)
    {
        var warehouses = await _repository.GetAllAsync(companyId);
        return warehouses.Select(w => new WarehouseContractDto(
            WarehouseCen: w.WarehouseCen,
            Name: w.Name,
            Description: w.Address,
            IsActive: w.IsActive
        ));
    }
}