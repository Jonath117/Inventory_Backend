using Purchases.Domain.Entities;

namespace Purchases.Application.Interfaces.Repositories;

public interface IPurchasesRepository
{
    Task<Purchase> CreateAsync(int companyId, Purchase purchase);
    
    Task<(int TotalCount, IEnumerable<Purchase> Items)> GetPagedOrdersAsync(
        int companyId, 
        int? status, 
        int page, 
        int pageSize, 
        bool sortDescending, 
        CancellationToken cancellationToken);
}  