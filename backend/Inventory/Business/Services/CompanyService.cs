using Microsoft.EntityFrameworkCore;
using WebApp1.Domain.DTOs;
using WebApp1.Domain.Interfaces;
using WebApp1.Infrastructure.Data;

namespace WebApp1.Business.Services;

public class CompanyService: ICompanyService
{
    private readonly ApplicationDbContext _context;

    public CompanyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CompanyDto>> GetCompanyAsync()
    {
        var companies = await _context.Companies
            .Where(c => c.IsActive)
            .Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name
            })
             .ToListAsync();

        if (companies.Count == 0)
        {
            throw new Exception("No se encontraron empresas activas");
        }
        return companies;
    }
}