using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;
using Sales.Domain.Enums;

namespace Sales.Application.Features.Kds;

public record GetKdsItemsQuery(int CompanyId, string TeamCen) : IRequest<IEnumerable<KdsItemContractResponse>>;

public class GetKdsItemsQueryHandler : IRequestHandler<GetKdsItemsQuery, IEnumerable<KdsItemContractResponse>>
{
    private readonly ISalesRepository _repository;

    public GetKdsItemsQueryHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<KdsItemContractResponse>> Handle(GetKdsItemsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _repository.GetDailyTicketsAsync(request.CompanyId, cancellationToken);
        var openTickets = tickets.Where(t => t.Status == Domain.Enums.TicketStatus.Open).ToList();

        var result = new List<KdsItemContractResponse>();
        foreach (var ticket in openTickets)
        {
            foreach (var item in ticket.ComandaItems)
            {
                var ticketItem = ticket.Items.First(i => i.TicketItemCen == item.TicketItemCen);
    
                result.Add(new KdsItemContractResponse(
                    TicketItemCen: item.TicketItemCen, 
                    TicketCen: ticket.TicketCen,
                    ProductName: ticketItem.ProductName,
                    Quantity: (int)ticketItem.Quantity,
                    Note: ticketItem.Note ?? string.Empty,
                    Status: item.Status.ToString(),
                    SentAt: item.SentAt,
                    Station: item.Station.ToString()
                ));
            }
        }

        return result;
    }
}