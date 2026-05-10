using Sales.Domain.Entities;

namespace Sales.Domain.Interfaces;

public interface ISalesRepository
{
    Task<Ticket> AddTicketAsync(Ticket ticket, CancellationToken cancellationToken);
    Task<string> GenerateTicketNumberAsync(int companyId, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}