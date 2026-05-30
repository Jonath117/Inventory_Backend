using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IGetProductKardexService
{
    Task<IEnumerable<KardexMovementContractDto>> GetProductKardexAsync(int companyId, string productCen, string? warehouseCen, DateTime? from = null, DateTime? to = null);
}