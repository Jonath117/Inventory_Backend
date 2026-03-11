using WebApp1.Domain.DTOs;

namespace WebApp1.Domain.Interfaces;

public interface IGetProductKardexService
{
    Task<List<MovementHistoryDto>> GetProductKardexAsync(int companyId, int productId, int? warehouseId = null);
}