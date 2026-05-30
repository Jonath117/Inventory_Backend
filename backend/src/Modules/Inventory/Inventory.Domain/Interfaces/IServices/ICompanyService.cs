using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface ICompanyService
{
    Task<IEnumerable<CompanyContractDto>> GetCompanyAsync();
    Task<CompanyLookupContractDto> GetCompanyByCenAsync(string companyCen);
}