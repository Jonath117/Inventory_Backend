using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;

namespace Sales.Application.Features.Tickets;

public record SendToKdsCommand(int CompanyId, string TicketCen) : IRequest<IEnumerable<TicketItemContractResponse>?>;

public class SendToKdsCommandHandler : IRequestHandler<SendToKdsCommand, IEnumerable<TicketItemContractResponse>?>
{
    private readonly ISalesRepository _repository;

    public SendToKdsCommandHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TicketItemContractResponse>?> Handle(SendToKdsCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByCenAsync(request.CompanyId, request.TicketCen, cancellationToken);
        if (ticket == null) return null;

        foreach (var item in ticket.Items)
        {
            ticket.SendToKds(item.TicketItemCen, Domain.Enums.KdsStation.Kitchen);
        }

        await _repository.SaveChangesAsync(cancellationToken);

        return ticket.Items.Select(i => new TicketItemContractResponse(
            TicketItemCen: i.TicketItemCen,
            ProductCen: i.ProductCen,
            ProductName: i.ProductName,
            Quantity: (int)i.Quantity,
            UnitPrice: i.UnitPrice,
            SubTotal: i.SubTotal,
            Note: i.Note,
            Status: "Sent"
        ));
    }
}