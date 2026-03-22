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

    public async Task<List<StockProductDto>> GetCurrentStockAsync(int companyId, int? warehouseId = null)
    {
        var stocks = await _repository.GetStockAsync(companyId, warehouseId);
        
        return stocks.Select(s => new StockProductDto
        (
            s.ProductId,
            s.Product!.Sku,
            s.Product.Name,
            s.Warehouse!.Name,
            s.CurrentStock,
            s.Product.Unit?.Name ?? "Sin unidad",
            s.Product.MinStockAlert,
            s.LastUpdated
        )).ToList();
    }
    
}