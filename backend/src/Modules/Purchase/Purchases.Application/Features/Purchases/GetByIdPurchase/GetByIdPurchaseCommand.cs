using MediatR;

namespace Purchases.Application.Features.Purchases.GetByIdPurchase;

public record PurchaseOrderDetailItemDto(string ProductCen, int Quantity);

public record PurchaseOrderDetailDto(
    string OrderCen, 
    int Status, 
    DateTime CreatedAt, 
    DateTime? ConfirmedAt, 
    string SupplierCen, 
    string WarehouseCen, 
    List<PurchaseOrderDetailItemDto> Items
);

public record GetPurchaseOrderDetailQuery(int CompanyId, string OrderCen) : IRequest<PurchaseOrderDetailDto>;