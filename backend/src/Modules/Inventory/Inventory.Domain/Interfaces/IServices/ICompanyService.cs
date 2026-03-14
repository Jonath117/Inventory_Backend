using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface ICompanyService
{
    public Task<List<CompanyDto>> GetCompanyAsync();
}