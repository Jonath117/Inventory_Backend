using MediatR;

namespace Purchases.Application.Features.Purchases.ConfirmPurchase;

public record PurchaseOrderConfirmationDto(
    string OrderCen, 
    int Status, 
    DateTime ConfirmedAt
);

public record ConfirmPurchaseOrderCommand(
    int CompanyId, 
    string CompanyCen, 
    string OrderCen
) : IRequest<PurchaseOrderConfirmationDto>;