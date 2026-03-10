using Microsoft.EntityFrameworkCore;
using WebApp1.Domain.DTOs;
using WebApp1.Domain.Entities;
using WebApp1.Domain.Interfaces;
using WebApp1.Infrastructure.Data;

namespace WebApp1.Business.Services;

public class AdjustmentService: IAdjustmentService
{
    
    private readonly ApplicationDbContext _context;

    public AdjustmentService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    public async Task RegisterAdjustmentAsync(int companyId, AdjustmentDto adjustment)
    {
        if(adjustment.Quantity == 0) throw new Exception("La cantidad no puede ser cero");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var stockRecord = await _context.InventoryStocks
                .FirstOrDefaultAsync(s => s.ProductId == adjustment.ProductId && s.WarehouseId == adjustment.WarehouseId);

            decimal previousStock = stockRecord?.CurrentStock ?? 0;
            decimal newStock = previousStock + adjustment.Quantity;

            if (newStock < 0) throw new Exception($"El stock no puede quedar en negativo: Stock actual: ${previousStock}");

            if (stockRecord == null)
            {
                stockRecord = new InventoryStock
                {
                    CompanyId = companyId,
                    ProductId = adjustment.ProductId,
                    WarehouseId = adjustment.WarehouseId,
                    CurrentStock = newStock,
                    LastUpdated = DateTime.UtcNow
                };
                _context.InventoryStocks.Add(stockRecord);
            }
            else
            {
                stockRecord.CurrentStock = newStock;
                stockRecord.LastUpdated = DateTime.UtcNow;
            }

            var movement = new InventoryMovement
            {
                CompanyId = companyId,
                ProductId = adjustment.ProductId,
                WarehouseId = adjustment.WarehouseId,
                MovementType = "Ajuste",
                Quantity = adjustment.Quantity,
                PreviousStock = previousStock,
                NewStock = newStock,
                Reason = adjustment.Reason,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.InventoryMovements.Add(movement);

            await _context.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }   
    }
}