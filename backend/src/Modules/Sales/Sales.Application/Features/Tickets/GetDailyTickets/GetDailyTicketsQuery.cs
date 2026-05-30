using MediatR;
using Sales.Application.Interfaces;

namespace Sales.Application.Features.Tickets;

public record GetDailyTicketsQuery(int CompanyId) : IRequest<IEnumerable<TicketContractResponse>>;

public class GetDailyTicketsQueryHandler : IRequestHandler<GetDailyTicketsQuery, IEnumerable<TicketContractResponse>>
{
    private readonly ISalesRepository _repository;

    public GetDailyTicketsQueryHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TicketContractResponse>> Handle(GetDailyTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _repository.GetDailyTicketsAsync(request.CompanyId, cancellationToken);

        return tickets.Select(t => new TicketContractResponse(
            TicketCen: t.TicketCen,
            Date: t.CreatedAt,
            Status: t.Status.ToString(),
            WaiterName: t.WaiterName,
            SubTotal: t.SubTotal,
            TaxAmount: t.TaxAmount,
            Total: t.Total,
            CustomerName: t.CustomerName,
            WarehouseCen: t.WarehouseCen
        ));
    }
}