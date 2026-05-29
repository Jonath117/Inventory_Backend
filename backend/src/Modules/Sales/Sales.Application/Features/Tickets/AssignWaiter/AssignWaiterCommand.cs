using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;
using Sales.Domain.Exceptions;

namespace Sales.Application.Features.Tickets;

public record AssignWaiterCommand(int CompanyId, string TicketCen, AssignTicketWaiterContractRequest Request) : IRequest<AssignTicketWaiterContractResponse>;

public class AssignWaiterCommandHandler : IRequestHandler<AssignWaiterCommand, AssignTicketWaiterContractResponse>
{
    private readonly ISalesRepository _repository;

    public AssignWaiterCommandHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<AssignTicketWaiterContractResponse> Handle(AssignWaiterCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByCenAsync(request.CompanyId, request.TicketCen, cancellationToken);
        if (ticket == null)
        {
            throw new NotFoundException("Ticket", request.TicketCen);
        }

        string waiterName = $"{request.Request.WaiterCen}";
        
        ticket.AssignWaiter(request.Request.WaiterCen, waiterName);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new AssignTicketWaiterContractResponse(
            TicketCen: ticket.TicketCen,
            WaiterCen: ticket.WaiterCen!,
            WaiterName: ticket.WaiterName!
        );
    }
}