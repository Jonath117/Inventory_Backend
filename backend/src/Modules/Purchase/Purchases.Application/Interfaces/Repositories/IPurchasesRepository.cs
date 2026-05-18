using Purchases.Domain.Entities;

namespace Purchases.Application.Interfaces.Repositories;

public interface IPurchasesRepository
{
    Task<Purchase> CreateAsync(int companyId, Purchase purchase);
}  