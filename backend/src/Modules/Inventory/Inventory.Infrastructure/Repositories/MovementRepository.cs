using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventory.Infrastructure.Repositories;

public class MovementRepository : IMovementRepository
{
    private readonly InventoryDbContext _context;
    private IDbContextTransaction? _transaction;
    
    public MovementRepository(InventoryDbContext context)
    {
        _context = context;
    }

    
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }
    
    public async Task CommitTransactionAsync()
    {
        if(_transaction != null)
            await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if(_transaction != null)
            await _transaction.RollbackAsync();
    }

    public async Task<InventoryStock?> GetStockAsync(int companyId, int productId, int warehouseId)
    {
        return await _context.InventoryStocks
            .FirstOrDefaultAsync(s =>
                s.CompanyId == companyId &&
                s.ProductId == productId &&
                s.WarehouseId == warehouseId);
    }

    public async Task AddStockAsync(InventoryStock stock)
    {
        await _context.InventoryStocks.AddAsync(stock);
    }

    public Task UpdateStockAsync(InventoryStock stock)
    { 
        _context.InventoryStocks.Update(stock);
        return Task.CompletedTask;
    }

    public async Task AddMovementAsync(InventoryMovement movement)
    {
        await _context.InventoryMovements.AddAsync(movement);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<decimal> GetTotalStockAsync(int companyId, int productId)
    {
        return await _context.InventoryStocks
            .Where(s => s.CompanyId == companyId && s.ProductId == productId)
            .SumAsync(s => s.CurrentStock);
    }
}