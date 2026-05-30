using MediatR;
using Purchases.Domain.Entities;

namespace Purchases.Application.Features.Purchases.CreatePurchase;


public record CreatePurchaseItemDto(string ProductCen, int Quantity);

public record CreatePurchaseCommand(
    int CompanyId, 
    string SupplierCen,     
    string WarehouseCen, 
    List<CreatePurchaseItemDto> Items
) : IRequest<PurchaseOrderSummaryResponse>;

public record PurchaseOrderSummaryResponse(string OrderCen, int Status);

public record CreatePurchaseRequestDto(
    string SupplierCen, 
    string WarehouseCen, 
    List<CreatePurchaseRequestItemDto> Items
);

public record CreatePurchaseRequestItemDto(
    string ProductCen, 
    int Quantity
);