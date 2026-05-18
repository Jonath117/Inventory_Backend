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
}