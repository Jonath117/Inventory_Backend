using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;


namespace Inventory.Application.Services;

public class CompanyService: ICompanyService
{
    private readonly ICompanyRepository _repository;

    public CompanyService(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CompanyContractDto>> GetCompanyAsync()
    {
        var companies = await _repository.GetActiveCompaniesAsync();

        return companies.Select(c => new CompanyContractDto(
            CompanyCen: c.Cen,
            Name: c.Name,
            IsActive: c.IsActive
        ));
    }

    public async Task<CompanyLookupContractDto> GetCompanyByCenAsync(string companyCen)
    {
        var company = await _repository.GetByCenAsync(companyCen);
        if (company == null)
        {
            throw new KeyNotFoundException($"Company with CEN {companyCen} not found.");
        }

        return new CompanyLookupContractDto(
            CompanyId: company.Id,
            CompanyCen: company.Cen,
            Name: company.Name
        );
    }
}