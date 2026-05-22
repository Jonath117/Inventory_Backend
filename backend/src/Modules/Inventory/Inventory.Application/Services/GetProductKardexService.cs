using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Application.Services;

public class GetProductKardexService :  IGetProductKardexService
{
    private readonly IGetProductKardexRepository _repository;

    public GetProductKardexService(IGetProductKardexRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<KardexMovementContractDto>> GetProductKardexAsync(int companyId, string productCen, string? warehouseCen, DateTime? from = null, DateTime? to = null)
    {
        var movements = await _repository.GetProductKardexAsync(companyId, productCen, warehouseCen, from, to);
        
        return movements.Select(m => new KardexMovementContractDto(
            MovementCen: m.MovementCen,
            MovementType: m.MovementType,
            Date: m.CreatedAt,
            WarehouseCen: m.Warehouse!.WarehouseCen,
            WarehouseName: m.Warehouse!.Name,
            Quantity: (double)m.Quantity,
            Balance: (double)m.NewStock,
            Source: null, // Depending on where source is stored, maybe m.Source if exists
            ReferenceCen: m.Reference
        ));
    }
}