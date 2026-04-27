using MediatR;
using Sales.Domain.Entities;
using Sales.Domain.Interfaces;

namespace Sales.Application.Features.Tickets.CreateTicket;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, int>
{
    private readonly ISalesRepository _repository;

    public CreateTicketCommandHandler(ISalesRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<int> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticketNumber = await _repository.GenerateTicketNumberAsync(request.CompanyId, cancellationToken);

        decimal globalTextRate = 13.00m;
        var ticket = Ticket.CreateOpenTicket(request.CompanyId, ticketNumber, globalTextRate);
        
        await _repository.AddTicketAsync(ticket, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        
        return ticket.Id;
    }
}   