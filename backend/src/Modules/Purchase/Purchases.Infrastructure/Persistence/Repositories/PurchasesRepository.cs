using Microsoft.EntityFrameworkCore;
using Purchases.Application.Interfaces.Repositories;
using Purchases.Domain.Entities;

namespace Purchases.Infrastructure.Persistence.Repositories;

public class PurchasesRepository : IPurchasesRepository
{
    private readonly PurchaseDbContext _context;

    public PurchasesRepository(PurchaseDbContext context)
    {
        _context = context;
    }

    public async Task<Purchase> CreateAsync(int companyId, Purchase purchase)
    {
        await _context.Purchases.AddAsync(purchase);
        await _context.SaveChangesAsync();
        return purchase;
    }

    public async Task<(int TotalCount, IEnumerable<Purchase> Items)> GetPagedOrdersAsync(int companyId, int? status, int page, int pageSize, bool sortDescending,
        CancellationToken cancellationToken)
    {
        var query = _context.Purchases
            .AsNoTracking()
            .Include(p => p.Items) 
            .Where(p => p.CompanyId == companyId);

        if (status.HasValue)
        {
            query = query.Where(p => (int)p.Status == status.Value);
        }

        query = sortDescending 
            ? query.OrderByDescending(p => p.CreatedAt) 
            : query.OrderBy(p => p.CreatedAt);

        int totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (totalCount, items);
    }

    public async Task<Purchase?> GetByOrderCenAsync(int companyId, string orderCen, CancellationToken cancellationToken)
    {
        return await _context.Purchases
            .AsNoTracking()
            .Include(p => p.Items) 
            .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.OrderCen == orderCen, cancellationToken);
    }
}