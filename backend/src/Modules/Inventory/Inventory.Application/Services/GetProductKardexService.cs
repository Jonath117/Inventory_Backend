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

    public async Task<List<MovementHistoryDto>> GetProductKardexAsync(int companyId, int productId, int? warehouseId = null)
    {
        var movements = await _repository
            .GetMovementsAsync(companyId, productId, warehouseId);
        
        return movements.Select(m => new MovementHistoryDto
        {
            Id = m.Id,
            Date = m.CreatedAt,
            MovementType = m.MovementType,
            Quantity = m.Quantity,
            PreviousStock = m.PreviousStock,
            NewStock = m.NewStock,
            Reason = m.Reason,
            Reference = m.Reference,
            WareHouseName = m.Warehouse!.Name
        }).ToList();
    }
}