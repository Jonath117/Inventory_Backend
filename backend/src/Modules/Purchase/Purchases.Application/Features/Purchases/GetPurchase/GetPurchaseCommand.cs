using MediatR;

namespace Purchases.Application.Features.Purchases.GetPurchase;

public record PurchaseOrderListDto(
    string OrderCen,
    int Status,
    DateTime CreatedAt,
    DateTime? ConfirmedAt,
    string SupplierCen,
    int ItemCount);
public record PagedResultDtoOfPurchaseOrderListDto(
    List<PurchaseOrderListDto> Items,
    int TotalCount,
    int TotalPages,
    int CurrentPage);

public record GetPurchaseOrdersQuery(
    int CompanyId,
    int? Status,
    int Page,
    int PageSize,
    bool SortDescending) : IRequest<PagedResultDtoOfPurchaseOrderListDto>;