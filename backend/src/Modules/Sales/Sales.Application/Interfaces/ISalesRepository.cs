using Sales.Domain.Entities;

namespace Sales.Application.Interfaces;

public interface ISalesRepository
{
    Task<Ticket> AddTicketAsync(Ticket ticket, CancellationToken cancellationToken);
    Task<string> GenerateTicketNumberAsync(int companyId, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}