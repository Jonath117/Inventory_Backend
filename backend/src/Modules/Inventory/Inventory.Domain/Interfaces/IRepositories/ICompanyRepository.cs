using Shared.Domain;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface ICompanyRepository
{
    Task<List<Company>> GetActiveCompaniesAsync();
}