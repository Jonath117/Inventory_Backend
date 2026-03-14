using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;
namespace Inventory.Application.Services;

public class GetStockService: IGetStockService
{
    private readonly IGetStockRepository _repository;

    public GetStockService(IGetStockRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<StockDto>> GetCurrentStockAsync(int companyId, int? warehouseId = null)
    {
        var stocks = await _repository.GetStockAsync(companyId, warehouseId);
        
        return stocks.Select(s => new StockDto
        {
            ProductId = s.ProductId,
            Sku = s.Product!.Sku,
            ProductName = s.Product.Name,
            WarehouseName = s.Warehouse!.Name,
            CurrentStock = s.CurrentStock,
            UnitOfMeasure = s.Product.UnitOfMeasure,
            MinStockAlert = s.Product.MinStockAlert,
            LastUpdated =  s.LastUpdated,
        }).ToList();
    }
    
}