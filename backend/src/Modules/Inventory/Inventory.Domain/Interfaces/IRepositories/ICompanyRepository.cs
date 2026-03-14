using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface ICompanyRepository
{
    Task<List<Company>> GetActiveCompaniesAsync();
}