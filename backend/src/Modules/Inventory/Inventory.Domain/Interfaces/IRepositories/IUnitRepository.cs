using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface IUnitRepository
{
    Task<IEnumerable<Unit>> GetUnitsAsync(int companyId);
    
    Task<bool> ExistsByNameAsync(int companyId, string name);
    
    Task<Unit> AddAsync(Unit unit);
}