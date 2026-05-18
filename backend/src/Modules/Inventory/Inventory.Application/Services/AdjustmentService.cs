using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Application.Services;

public class AdjustmentService : IAdjustmentService
{
    private readonly IAdjustmentRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public AdjustmentService(IAdjustmentRepository repository, IProductRepository productRepository, IWarehouseRepository warehouseRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task<InventoryAdjustmentContractResponse> RegisterAdjustmentAsync(int companyId, InventoryAdjustmentContractRequest request)
    {
        var warehouseInfo = await _warehouseRepository.GetInfoByCenAsync(companyId, request.WarehouseCen);
        if (warehouseInfo.Id == 0) throw new ArgumentException("Bodega no válida");

        var generatedMovements = new List<GeneratedMovementContractDto>();
        string adjustmentCen = $"ADJ-{Guid.NewGuid():N}";

        await _repository.BeginTransactionAsync();
        try
        {
            foreach (var line in request.Lines)
            {
                if (line.Quantity <= 0) throw new ArgumentException("La cantidad no puede ser cero o negativa");
                if (line.AdjustmentType != "IN" && line.AdjustmentType != "OUT") throw new ArgumentException("Tipo debe ser IN u OUT");

                var productInfo = await _productRepository.GetProductInfoByCenAsync(companyId, line.ProductCen);
                if (productInfo.Id == 0) throw new ArgumentException($"Producto {line.ProductCen} no válido");

                var stockRecord = await _repository.GetStockAsync(productInfo.Id, warehouseInfo.Id);
                decimal previousStock = stockRecord?.CurrentStock ?? 0;
                
                decimal adjustmentQty = line.AdjustmentType == "IN" ? line.Quantity : -line.Quantity;
                decimal newStock = previousStock + adjustmentQty;

                if (newStock < 0) throw new InvalidOperationException($"El ajuste dejaría el stock de {productInfo.Name} en negativo.");

                if (stockRecord == null)
                {
                    stockRecord = new InventoryStock(companyId, warehouseInfo.Id, productInfo.Id, newStock);
                    await _repository.AddStockAsync(stockRecord);
                }
                else
                {
                    stockRecord.AdjustStock(newStock);
                    await _repository.UpdateStockAsync(stockRecord);
                }

                var movement = new InventoryMovement(
                    companyId, warehouseInfo.Id, productInfo.Id, "AJUSTE_" + line.AdjustmentType, adjustmentQty, 
                    previousStock, newStock, request.Reason, null, adjustmentCen
                );
                
                await _repository.AddMovementAsync(movement);
                
                generatedMovements.Add(new GeneratedMovementContractDto(
                    movement.MovementCen, line.ProductCen, request.WarehouseCen, line.Quantity, line.AdjustmentType
                ));
            }

            await _repository.SaveChangesAsync();
            await _repository.CommitTransactionAsync();

            return new InventoryAdjustmentContractResponse(adjustmentCen, "COMPLETED", generatedMovements);
        }
        catch
        {
            await _repository.RollbackTransactionAsync();
            throw;
        }
    }
}