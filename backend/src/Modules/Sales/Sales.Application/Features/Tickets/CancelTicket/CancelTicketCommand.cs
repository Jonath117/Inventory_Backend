using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;

namespace Sales.Application.Features.Tickets;

public record CancelTicketCommand(int CompanyId, string TicketCen, CancelTicketContractRequest? Request) : IRequest<CancelTicketContractResponse?>;

public class CancelTicketCommandHandler : IRequestHandler<CancelTicketCommand, CancelTicketContractResponse?>
{
    private readonly ISalesRepository _repository;

    public CancelTicketCommandHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<CancelTicketContractResponse?> Handle(CancelTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByCenAsync(request.CompanyId, request.TicketCen, cancellationToken);
        if (ticket == null) return null;

        ticket.Cancel();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new CancelTicketContractResponse(
            TicketCen: ticket.TicketCen,
            Status: ticket.Status.ToString()
        );
    }
}