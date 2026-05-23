using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;

namespace Sales.Application.Features.Dashboard;

public record GetTopProductsQuery(int CompanyId, int TopN) : IRequest<IEnumerable<TopProductDashboardContractResponse>>;

public class GetTopProductsQueryHandler : IRequestHandler<GetTopProductsQuery, IEnumerable<TopProductDashboardContractResponse>>
{
    private readonly ISalesRepository _repository;

    public GetTopProductsQueryHandler(ISalesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TopProductDashboardContractResponse>> Handle(GetTopProductsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _repository.GetDailyTicketsAsync(request.CompanyId, cancellationToken);
        var paidTickets = tickets.Where(t => t.Status == Domain.Enums.TicketStatus.Paid).ToList();

        var topProducts = paidTickets
            .SelectMany(t => t.Items)
            .GroupBy(i => new { i.ProductCen, i.ProductName })
            .Select(g => new TopProductDashboardContractResponse(
                g.Key.ProductCen.ToString(),
                g.Key.ProductName,
                (int)g.Sum(i => i.Quantity),
                (double)g.Sum(i => i.SubTotal)
            ))
            .OrderByDescending(x => x.QuantitySold)
            .Take(request.TopN)
            .ToList();

        return topProducts;
    }
}