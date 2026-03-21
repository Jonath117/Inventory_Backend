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

    public async Task<List<CompanyDto>> GetCompanyAsync()
    {
        var companies = await _repository.GetActiveCompaniesAsync();

        if (companies == null || companies.Count == 0)
        {
            return new List<CompanyDto>();
        }

        return companies.Select(c => new CompanyDto
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();
    }
}