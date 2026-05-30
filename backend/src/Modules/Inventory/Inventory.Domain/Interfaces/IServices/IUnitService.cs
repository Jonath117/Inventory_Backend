using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IUnitService
{
    Task<IEnumerable<UnitContractDto>> GetUnitsAsync(int companyId);
    
    Task<UnitContractDto> CreateUnitAsync(int companyId, CreateUnitContractRequest request);
    Task<UnitContractDto> UpdateUnitAsync(int companyId, string unitCen, CreateUnitContractRequest request);
}