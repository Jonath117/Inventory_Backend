using MediatR;
using Purchases.Application.Interfaces.Repositories;

namespace Purchases.Application.Features.Purchases.GetPurchase;

public class GetPurchaseCommandHandler : IRequestHandler<GetPurchaseOrdersQuery, PagedResultDtoOfPurchaseOrderListDto>
{
    private readonly IPurchasesRepository _repository;

    public GetPurchaseCommandHandler(IPurchasesRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDtoOfPurchaseOrderListDto> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        var (totalCount, purchases) = await _repository.GetPagedOrdersAsync(
            request.CompanyId, 
            request.Status, 
            request.Page, 
            request.PageSize, 
            request.SortDescending, 
            cancellationToken);

        int totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var itemsDto = purchases.Select(p => new PurchaseOrderListDto(
            p.OrderCen,
            (int)p.Status,
            p.CreatedAt,
            p.ConfirmedAt,
            p.SupplierCen,
            p.Items.Count 
        )).ToList();

        return new PagedResultDtoOfPurchaseOrderListDto(itemsDto, totalCount, totalPages, request.Page);
    }
}