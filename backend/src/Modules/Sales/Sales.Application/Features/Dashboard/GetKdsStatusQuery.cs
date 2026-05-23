using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;

namespace Sales.Application.Features.Dashboard;

public record GetKdsStatusQuery(int CompanyId) : IRequest<KdsStatusDashboardDto>;

public class GetKdsStatusQueryHandler : IRequestHandler<GetKdsStatusQuery, KdsStatusDashboardDto>
{
    private readonly ISalesRepository _repository;

    public GetKdsStatusQueryHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<KdsStatusDashboardDto> Handle(GetKdsStatusQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _repository.GetDailyTicketsAsync(request.CompanyId, cancellationToken);
        var comandaItems = tickets.SelectMany(t => t.ComandaItems).ToList();

        return new KdsStatusDashboardDto(
            PendingItems: comandaItems.Count(i => i.Status == Domain.Enums.KdsItemStatus.Pending),
            PreparingItems: comandaItems.Count(i => i.Status == Domain.Enums.KdsItemStatus.Preparing),
            ReadyItems: comandaItems.Count(i => i.Status == Domain.Enums.KdsItemStatus.Ready)
        );
    }
}