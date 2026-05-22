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

    public async Task<IEnumerable<StockItemContractDto>> GetCurrentStockAsync(int companyId, string? productCen, string? warehouseCen)
    {
        var stocks = await _repository.GetCurrentStockAsync(companyId, productCen, warehouseCen);
        
        return stocks.Select(s => new StockItemContractDto
        (
            s.Product!.ProductCen,
            s.Product!.Sku,
            s.Product.Name,
            s.Warehouse!.WarehouseCen,
            s.Warehouse!.Name,
            (double)s.CurrentStock,
            s.Product.Unit?.Name ?? "Sin unidad",
            s.LastUpdated
        ));
    }
    
}