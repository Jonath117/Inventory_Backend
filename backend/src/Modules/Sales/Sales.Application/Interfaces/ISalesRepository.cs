using Sales.Domain.Entities;

namespace Sales.Application.Interfaces;

public interface ISalesRepository
{
    Task<Ticket> AddTicketAsync(Ticket ticket, CancellationToken cancellationToken);
    Task<string> GenerateTicketNumberAsync(int companyId, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Ticket?> GetByCenAsync(int companyId, string ticketCen, CancellationToken cancellationToken);
    Task<IEnumerable<Ticket>> GetDailyTicketsAsync(int companyId, CancellationToken cancellationToken);
}