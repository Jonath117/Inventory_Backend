using Microsoft.EntityFrameworkCore;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Persistence.Repositories;

public class SalesRepository : ISalesRepository
{
    private readonly SalesDbContext _context;

    public SalesRepository(SalesDbContext context)
    {
        _context = context;
    }
    
    public async Task<Ticket> AddTicketAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        await _context.Tickets.AddAsync(ticket, cancellationToken);
        return ticket;
    }

    public async Task<string> GenerateTicketNumberAsync(int companyId, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var count = await _context.Tickets
            .Where(t => t.CompanyId == companyId && t.CreatedAt >= today)
            .CountAsync(cancellationToken);

        return $"TCK-{today:yyyyMMMdd}-{(count + 1):D4}";
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}