using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Application.Services;

public class AdjustmentService: IAdjustmentService
{
    
    private readonly IAdjustmentRepository _repository;

    public AdjustmentService(IAdjustmentRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task RegisterAdjustmentAsync(int companyId, AdjustmentDto adjustment)
    {
        if(adjustment.Quantity == 0) throw new Exception("La cantidad no puede ser cero");

        await _repository.BeginTransactionAsync();

        try
        {
            var stockRecord = await _repository.GetStockAsync(adjustment.ProductId, adjustment.WarehouseId);

            decimal previousStock = stockRecord?.CurrentStock ?? 0;
            decimal newStock = previousStock + adjustment.Quantity;

            if (stockRecord == null)
            {
                stockRecord = new InventoryStock(companyId, adjustment.WarehouseId, adjustment.ProductId, newStock);
                await _repository.AddStockAsync(stockRecord);
            }
            else
            {
                stockRecord.AdjustStock(newStock);
            }

            var movement = new InventoryMovement(
                companyId,
                adjustment.WarehouseId,
                adjustment.ProductId,
                "Ajuste",
                adjustment.Quantity,
                previousStock,
                newStock,
                adjustment.Reason,
                null,
                null
            );
            
            await _repository.AddMovementAsync(movement);

            await _repository.SaveChangesAsync();
            
            await _repository.CommitTransactionAsync();
        }
        catch
        {
            await _repository.RollbackTransactionAsync();
            throw;
        }   
    }
}