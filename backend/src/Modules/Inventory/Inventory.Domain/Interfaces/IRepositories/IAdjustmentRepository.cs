using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface IAdjustmentRepository
{
    Task<InventoryStock?> GetStockAsync(int productId, int warehouseId);

    Task AddStockAsync(InventoryStock stock);

    Task UpdateStockAsync(InventoryStock stock);

    Task AddMovementAsync(InventoryMovement movement);

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();

    Task SaveChangesAsync();
}