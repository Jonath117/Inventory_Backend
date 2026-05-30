using MediatR;
using Sales.Application.Interfaces;

namespace Sales.Application.Features.Tickets;

public record GetTicketItemsQuery(int CompanyId, string TicketCen) : IRequest<IEnumerable<TicketItemContractResponse>>;

public class GetTicketItemsQueryHandler : IRequestHandler<GetTicketItemsQuery, IEnumerable<TicketItemContractResponse>>
{
    private readonly ISalesRepository _repository;

    public GetTicketItemsQueryHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TicketItemContractResponse>> Handle(GetTicketItemsQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByCenAsync(request.CompanyId, request.TicketCen, cancellationToken);

        if (ticket == null) return Enumerable.Empty<TicketItemContractResponse>();

        return ticket.Items.Select(i => {
            var comanda = ticket.ComandaItems.FirstOrDefault(c => c.TicketItemCen == i.TicketItemCen);

            return new TicketItemContractResponse(
                TicketItemCen: i.TicketItemCen,
                ProductCen: i.ProductCen,
                ProductName: i.ProductName,
                Quantity: (int)i.Quantity,
                UnitPrice: i.UnitPrice,
                SubTotal: i.SubTotal,
                Note: i.Note,
                Status: comanda != null ? comanda.Status.ToString() : "Added" 
            );
        });
    }
}