using MediatR;
using Sales.Application.Interfaces;

namespace Sales.Application.Features.Tickets;

public record GetTicketByCenQuery(int CompanyId, string TicketCen) : IRequest<TicketContractResponse?>;

public class GetTicketByCenQueryHandler : IRequestHandler<GetTicketByCenQuery, TicketContractResponse?>
{
    private readonly ISalesRepository _repository;

    public GetTicketByCenQueryHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<TicketContractResponse?> Handle(GetTicketByCenQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByCenAsync(request.CompanyId, request.TicketCen, cancellationToken);

        if (ticket == null) return null;

        return new TicketContractResponse(
            TicketCen: ticket.TicketCen,
            Date: ticket.CreatedAt,
            Status: ticket.Status.ToString(),
            WaiterName: ticket.WaiterName,
            SubTotal: ticket.SubTotal,
            TaxAmount: ticket.TaxAmount,
            Total: ticket.Total,
            CustomerName: ticket.CustomerName,
            WarehouseCen: ticket.WarehouseCen
        );
    }
}