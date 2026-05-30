using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;

namespace Sales.Application.Features.Dashboard;

public record GetDailySalesDashboardQuery(int CompanyId) : IRequest<DailySalesDashboardDto>;

public class GetDailySalesDashboardQueryHandler : IRequestHandler<GetDailySalesDashboardQuery, DailySalesDashboardDto>
{
    private readonly ISalesRepository _repository;

    public GetDailySalesDashboardQueryHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<DailySalesDashboardDto> Handle(GetDailySalesDashboardQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _repository.GetDailyTicketsAsync(request.CompanyId, cancellationToken);
        var paidTickets = tickets.Where(t => t.Status == Domain.Enums.TicketStatus.Paid).ToList();

        double totalSales = (double)paidTickets.Sum(t => t.Total);
        int count = paidTickets.Count;
        double average = count > 0 ? totalSales / count : 0;

        return new DailySalesDashboardDto(totalSales, count, average);
    }
}