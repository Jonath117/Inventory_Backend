using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

using System.Globalization;

namespace Inventory.Application.Services;

public class MovementService: IMovementService
{
    private readonly IMovementRepository _repository;
    private readonly IProductRepository _productRepository;

    public MovementService(IMovementRepository repository, IProductRepository productRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
    }

    public async Task RegisterMovement(int companyId, MovementDto request)
    {
        if (request.Quantity <= 0)
            throw new Exception("Cantidad debe ser mayor a cero");

        if (request.MovementType != "IN" && request.MovementType != "OUT")
            throw new Exception("El tipo de movimiento debe ser 'IN' o 'OUT'.");

        await _repository.BeginTransactionAsync();

        try
        {
            var stockRecord = await _repository.GetStockAsync(companyId, request.ProductId, request.WarehouseId);

            var previousStock = stockRecord?.CurrentStock ?? 0;
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
                    string stockFormateado = previousStock.ToString("0.##", CultureInfo.InvariantCulture);
                    string cantidadFormateada = request.Quantity.ToString("0.##", CultureInfo.InvariantCulture);

                    throw new Exception(
                        $"Stock insuficiente. Tienes {stockFormateado} y quieres sacar {cantidadFormateada}.");
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
                await _repository.AddStockAsync(stockRecord);
            }
            else
            {
                stockRecord.CurrentStock = newStock;
                stockRecord.LastUpdated = DateTime.UtcNow;
                
                await _repository.UpdateStockAsync(stockRecord);
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
            await _repository.AddMovementAsync(movement);
            await _repository.SaveChangesAsync();

            var product = await _productRepository.GetByIdAsync(companyId, request.ProductId);
            
            if (product != null)
            {
                var totalGlobalStock = await _repository.GetTotalStockAsync(companyId, request.ProductId);
                product.IsSoldOut = totalGlobalStock <= 0;
                
                await _productRepository.UpdateAsync(product);
            }
            
            await _repository.CommitTransactionAsync();
        }
        catch
        {
            await _repository.RollbackTransactionAsync();
            throw;
        }
        
    }
}