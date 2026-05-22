using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Application.Services;

public class MovementService : IMovementService
{
    private readonly IMovementRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public MovementService(IMovementRepository repository, IProductRepository productRepository, IWarehouseRepository warehouseRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task<StockConsumeContractResponse> ConsumeStockAsync(int companyId, StockConsumeContractRequest request)
    {
        var warehouseInfo = await _warehouseRepository.GetInfoByCenAsync(companyId, request.WarehouseCen);
        if (warehouseInfo.Id == 0) throw new ArgumentException("Bodega no válida");

        var requirements = new List<StockRequirementContractDto>();
        var movementsToSave = new List<InventoryMovement>();
        var generatedCens = new List<string>();
        
        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero");

            var productInfo = await _productRepository.GetProductInfoByCenAsync(companyId, item.ProductCen);
            if (productInfo.Id == 0) throw new ArgumentException($"Producto {item.ProductCen} no válido");

            var stockRecord = await _repository.GetStockAsync(companyId, productInfo.Id, warehouseInfo.Id);
            decimal currentStock = stockRecord?.CurrentStock ?? 0;

            if (currentStock < item.Quantity)
            {
                requirements.Add(new StockRequirementContractDto(
                    item.ProductCen, productInfo.Name, request.WarehouseCen, item.Quantity, currentStock, 
                    item.Quantity - currentStock, productInfo.UnitName, "Stock insuficiente"
                ));
            }
        }
        
        if (requirements.Any())
        {
            return new StockConsumeContractResponse(false, null, null, new List<string>(), requirements);
        }
        
        await _repository.BeginTransactionAsync();
        try
        {
            string documentCen = $"DOC-{Guid.NewGuid():N}"; 

            foreach (var item in request.Items)
            {
                var productInfo = await _productRepository.GetProductInfoByCenAsync(companyId, item.ProductCen);
                var stockRecord = await _repository.GetStockAsync(companyId, productInfo.Id, warehouseInfo.Id);
                
                decimal previousStock = stockRecord!.CurrentStock;
                decimal newStock = previousStock - item.Quantity;

                stockRecord.AdjustStock(newStock);
                await _repository.UpdateStockAsync(stockRecord);

                var movement = new InventoryMovement(
                    companyId, warehouseInfo.Id, productInfo.Id, "OUT", -item.Quantity, previousStock, newStock, 
                    request.Reason, request.ReferenceCen, documentCen
                );
                
                await _repository.AddMovementAsync(movement);
                generatedCens.Add(movement.MovementCen);
                
                var product = await _productRepository.GetByIdAsync(companyId, productInfo.Id);
                var totalGlobalStock = await _repository.GetTotalStockAsync(companyId, productInfo.Id);
                product!.MarkSoldOut(totalGlobalStock <= 0);
                await _productRepository.UpdateAsync(product);
            }

            await _repository.SaveChangesAsync();
            await _repository.CommitTransactionAsync();

            return new StockConsumeContractResponse(true, documentCen, request.Source, generatedCens, new List<StockRequirementContractDto>());
        }
        catch
        {
            await _repository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<string> IncreaseStockAsync(int companyId, StockIncreaseContractRequest request)
    {
        var warehouseInfo = await _warehouseRepository.GetInfoByCenAsync(companyId, request.WarehouseCen);
        if (warehouseInfo.Id == 0) throw new ArgumentException("Bodega no válida");

        await _repository.BeginTransactionAsync();
        try
        {
            string documentCen = $"DOC-{Guid.NewGuid():N}"; 

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero");

                var productInfo = await _productRepository.GetProductInfoByCenAsync(companyId, item.ProductCen);
                if (productInfo.Id == 0) throw new ArgumentException($"Producto {item.ProductCen} no válido");

                var stockRecord = await _repository.GetStockAsync(companyId, productInfo.Id, warehouseInfo.Id);
                decimal previousStock = stockRecord?.CurrentStock ?? 0;
                decimal newStock = previousStock + item.Quantity;

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
                    companyId, warehouseInfo.Id, productInfo.Id, "IN", item.Quantity, previousStock, newStock, 
                    request.Reason, request.ReferenceCen, documentCen
                );
                
                await _repository.AddMovementAsync(movement);

                var product = await _productRepository.GetByIdAsync(companyId, productInfo.Id);
                product!.MarkSoldOut(false); 
                await _productRepository.UpdateAsync(product);
            }

            await _repository.SaveChangesAsync();
            await _repository.CommitTransactionAsync();

            return documentCen; 
        }
        catch
        {
            await _repository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<InventoryDocumentContractDto> CreateDocumentAsync(int companyId, InventoryDocumentContractRequest request)
    {
        // For simplicity, we can use IncreaseStockAsync or ConsumeStockAsync logic here
        // But the contract asks for a unified Document creation.
        // For now, let's just implement it directly.
        
        var warehouseInfo = await _warehouseRepository.GetInfoByCenAsync(companyId, request.WarehouseCen);
        if (warehouseInfo.Id == 0) throw new ArgumentException("Bodega no válida");

        await _repository.BeginTransactionAsync();
        try
        {
            string documentCen = $"DOC-{Guid.NewGuid():N}";
            var generatedMovements = new List<GeneratedMovementContractDto>();

            foreach (var item in request.Items)
            {
                var productInfo = await _productRepository.GetProductInfoByCenAsync(companyId, item.ProductCen);
                if (productInfo.Id == 0) throw new ArgumentException($"Producto {item.ProductCen} no válido");

                var stockRecord = await _repository.GetStockAsync(companyId, productInfo.Id, warehouseInfo.Id);
                decimal previousStock = stockRecord?.CurrentStock ?? 0;
                
                decimal quantity = item.Quantity;
                if (request.DocumentType == "OUT" || request.DocumentType == "SALE") // Simplified logic
                {
                    quantity = -item.Quantity;
                }

                decimal newStock = previousStock + quantity;

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
                    companyId, warehouseInfo.Id, productInfo.Id, request.DocumentType, quantity, previousStock, newStock, 
                    request.Reason, request.ReferenceCen, documentCen
                );
                
                await _repository.AddMovementAsync(movement);
                
                generatedMovements.Add(new GeneratedMovementContractDto(
                    movement.MovementCen,
                    item.ProductCen,
                    request.WarehouseCen,
                    item.Quantity,
                    request.DocumentType
                ));
            }

            await _repository.SaveChangesAsync();
            await _repository.CommitTransactionAsync();

            return new InventoryDocumentContractDto(
                documentCen,
                request.DocumentType,
                DateTime.UtcNow,
                request.WarehouseCen,
                warehouseInfo.Name,
                request.Source,
                request.ReferenceCen,
                request.Reason,
                generatedMovements
            );
        }
        catch
        {
            await _repository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<IEnumerable<InventoryDocumentContractDto>> GetDocumentsAsync(int companyId, string? documentType, DateTime? from, DateTime? to)
    {
        var movements = await _repository.GetMovementsAsync(companyId, documentType, from, to);
        
        var grouped = movements.GroupBy(m => m.UserReference ?? m.MovementCen);
        
        return grouped.Select(g => {
            var first = g.First();
            return new InventoryDocumentContractDto(
                g.Key,
                first.MovementType,
                first.CreatedAt,
                first.Warehouse?.WarehouseCen ?? string.Empty,
                first.Warehouse?.Name ?? "N/A",
                "N/A", 
                first.Reference ?? string.Empty,
                first.Reason,
                g.Select(m => new GeneratedMovementContractDto(
                    m.MovementCen,
                    m.Product?.ProductCen ?? string.Empty,
                    m.Warehouse?.WarehouseCen ?? string.Empty,
                    m.Quantity,
                    m.MovementType
                )).ToList()
            );
        });
    }

    public async Task<StockValidationContractResponse> ValidateStockAsync(int companyId, StockValidationContractRequest request)
    {
        var warehouseInfo = await _warehouseRepository.GetInfoByCenAsync(companyId, request.WarehouseCen);
        if (warehouseInfo.Id == 0) throw new ArgumentException("Bodega no válida");

        var requirements = new List<StockRequirementContractDto>();
        
        foreach (var item in request.Items)
        {
            var productInfo = await _productRepository.GetProductInfoByCenAsync(companyId, item.ProductCen);
            if (productInfo.Id == 0) continue;

            var stockRecord = await _repository.GetStockAsync(companyId, productInfo.Id, warehouseInfo.Id);
            decimal currentStock = stockRecord?.CurrentStock ?? 0;

            if (currentStock < item.Quantity)
            {
                requirements.Add(new StockRequirementContractDto(
                    item.ProductCen, productInfo.Name, request.WarehouseCen, item.Quantity, currentStock, 
                    item.Quantity - currentStock, productInfo.UnitName, "Stock insuficiente"
                ));
            }
        }

        return new StockValidationContractResponse(!requirements.Any(), requirements);
    }

    public Task RegisterMovement(int companyId, MovementDto request)
    {
        throw new NotImplementedException();
    }
}