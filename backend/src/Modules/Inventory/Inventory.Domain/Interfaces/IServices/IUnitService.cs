using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IUnitService
{
    Task<IEnumerable<UnitLookUpDto>> GetUnitsAsync(int companyId);
    
    Task<UnitLookUpDto> CreateUnitAsync(UnitCreateDto dto);
}