using WebApp1.Domain.DTOs;


namespace WebApp1.Domain.Interfaces;

public interface ICompanyService
{
    public Task<List<CompanyDto>> GetCompanyAsync();
}