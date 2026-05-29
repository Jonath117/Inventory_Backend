using MediatR;
using Sales.Application.Interfaces;
using Sales.Domain.Exceptions;

namespace Sales.Application.Features.Tickets;

public record GetTicketTotalsQuery(int CompanyId, string TicketCen) : IRequest<TicketTotalsContractResponse>;

public class GetTicketTotalsQueryHandler : IRequestHandler<GetTicketTotalsQuery, TicketTotalsContractResponse>
{
    private readonly ISalesRepository _repository;

    public GetTicketTotalsQueryHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<TicketTotalsContractResponse> Handle(GetTicketTotalsQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByCenAsync(request.CompanyId, request.TicketCen, cancellationToken);

        if (ticket == null)
        {
            throw new NotFoundException("Ticket", request.TicketCen);
        }

        return new TicketTotalsContractResponse(
            SubTotal: ticket.SubTotal,
            TaxAmount: ticket.TaxAmount,
            Total: ticket.Total,
            TaxRate: ticket.TaxRate
        );
    }
}