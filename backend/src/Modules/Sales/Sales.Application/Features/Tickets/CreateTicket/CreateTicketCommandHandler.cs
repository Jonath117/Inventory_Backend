using MediatR;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Application.Features.Tickets;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketContractResponse>
{
    private readonly ISalesRepository _repository;

    public CreateTicketCommandHandler(ISalesRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<TicketContractResponse> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticketNumber = await _repository.GenerateTicketNumberAsync(request.CompanyId, cancellationToken);
        var ticketCen = $"TCK-{Guid.NewGuid().ToString().Substring(0, 8)}";

        decimal globalTextRate = 13.00m;
        var ticket = Ticket.CreateOpenTicket(request.CompanyId, ticketCen, ticketNumber, globalTextRate, request.WarehouseCen);
        
        if (!string.IsNullOrEmpty(request.WaiterCen))
        {
             ticket.AssignWaiter(request.WaiterCen, $"Waiter {request.WaiterCen}");
        }

        await _repository.AddTicketAsync(ticket, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        
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