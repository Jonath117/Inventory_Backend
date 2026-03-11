using WebApp1.Domain.DTOs;

namespace WebApp1.Domain.Interfaces;

public interface IGetStockService
{
    Task<List<StockDto>> GetCurrentStockAsync(int companyId, int? warehouseId = null);
}