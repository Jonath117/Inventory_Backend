using Microsoft.EntityFrameworkCore;
using WebApp1.Domain.DTOs;
using WebApp1.Domain.Entities;
using WebApp1.Domain.Interfaces;
using WebApp1.Infrastructure.Data;

namespace WebApp1.Business.Services;

public class MovementService: IMovementService
{
    private readonly ApplicationDbContext _context;

    public MovementService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task RegisterMovement(int companyId, MovementDto request)
    {
        if (request.Quantity <= 0)
            throw new Exception("Cantidad debe ser mayor a cero");

        if (request.MovementType != "IN" && request.MovementType != "OUT")
            throw new Exception("El tipo de movimiento debe ser 'IN' o 'OUT'.");
        
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var stockRecord = await _context.InventoryStocks
                .FirstOrDefaultAsync(s => s.ProductId == request.ProductId
                                          && s.WarehouseId == request.WarehouseId);

            decimal previousStock = stockRecord?.CurrentStock ?? 0;
            decimal newStock;

            if (request.MovementType == "IN")
            {
                newStock = previousStock + request.Quantity;
            }
            else
            {
                newStock = previousStock - request.Quantity;

                if (newStock < 0)
                {
                    throw new Exception(
                        $"Stock insuficiente. Tienes {previousStock} y quieres sacar {request.Quantity}.");
                }
            }

            if (stockRecord == null)
            {
                stockRecord = new InventoryStock
                {
                    CompanyId = companyId,
                    ProductId = request.ProductId,
                    WarehouseId = request.WarehouseId,
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
                ProductId = request.ProductId,
                WarehouseId = request.WarehouseId,
                MovementType = request.MovementType,
                Quantity = request.MovementType == "IN" ? request.Quantity : -request.Quantity,
                PreviousStock = previousStock,
                NewStock = newStock,
                Reason = request.Reason,
                Reference = request.Reference,
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