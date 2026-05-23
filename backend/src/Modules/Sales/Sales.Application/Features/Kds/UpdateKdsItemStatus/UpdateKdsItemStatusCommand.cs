using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;

namespace Sales.Application.Features.Kds.UpdateKdsItemStatus;

public record UpdateKdsItemStatusCommand(int CompanyId, string TicketItemCen, UpdateKdsItemStatusContractRequest Request) : IRequest<bool>;

public class UpdateKdsItemStatusCommandHandler : IRequestHandler<UpdateKdsItemStatusCommand, bool>
{
    private readonly ISalesRepository _repository;

    public UpdateKdsItemStatusCommandHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateKdsItemStatusCommand request, CancellationToken cancellationToken)
    {
        var tickets = await _repository.GetDailyTicketsAsync(request.CompanyId, cancellationToken);
    
        foreach (var ticket in tickets)
        {
            var item = ticket.ComandaItems.FirstOrDefault(i => i.TicketItemCen == request.TicketItemCen);
        
            if (item != null)
            {
                if (request.Request.Status.Equals("preparing", StringComparison.OrdinalIgnoreCase))
                    item.MarkAsPreparing();
                else if (request.Request.Status.Equals("ready", StringComparison.OrdinalIgnoreCase))
                    item.MarksAsReady(); 
            
                await _repository.SaveChangesAsync(cancellationToken);
                return true;
            }
        }

        return false;
    }
}