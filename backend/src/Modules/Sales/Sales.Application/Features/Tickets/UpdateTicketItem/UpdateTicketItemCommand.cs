using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;
using Sales.Domain.Exceptions;

namespace Sales.Application.Features.Tickets;

public record UpdateTicketItemCommand(int CompanyId, string TicketCen, string TicketItemCen, UpdateTicketItemContractRequest Request) : IRequest<TicketItemContractResponse>;

public record UpdateTicketItemContractRequest(
    int Quantity,
    string? Note
);

public class UpdateTicketItemCommandHandler : IRequestHandler<UpdateTicketItemCommand, TicketItemContractResponse>
{
    private readonly ISalesRepository _repository;

    public UpdateTicketItemCommandHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<TicketItemContractResponse> Handle(UpdateTicketItemCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByCenAsync(request.CompanyId, request.TicketCen, cancellationToken);
        if (ticket == null)
        {
            throw new NotFoundException("Ticket", request.TicketCen);
        }

        var item = ticket.Items.FirstOrDefault(i => i.TicketItemCen == request.TicketItemCen);
        if (item == null)
        {
            throw new NotFoundException("Ítem de Ticket", request.TicketItemCen);
        }

        ticket.UpdateItemQuantity(item.ProductCen, request.Request.Quantity);

        if (request.Request.Note != null)
        {
            ticket.UpdateItemNote(item.ProductCen, request.Request.Note);
        }

        await _repository.SaveChangesAsync(cancellationToken);

        return new TicketItemContractResponse(
            TicketItemCen: item.TicketItemCen,
            ProductCen: item.ProductCen,
            ProductName: item.ProductName,
            Quantity: (int)item.Quantity,
            UnitPrice: item.UnitPrice,
            SubTotal: item.SubTotal,
            Note: item.Note,
            Status: "Updated"
        );
    }
}