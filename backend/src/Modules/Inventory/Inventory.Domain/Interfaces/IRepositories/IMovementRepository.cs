using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface IMovementRepository
{
    Task<InventoryStock?> GetStockAsync(int companyId, int productId, int warehouseId);

    Task AddStockAsync(InventoryStock stock);

    Task UpdateStockAsync(InventoryStock stock);

    Task AddMovementAsync(InventoryMovement movement);

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();

    Task SaveChangesAsync();
    
    Task<decimal> GetTotalStockAsync(int companyId, int productId);
}