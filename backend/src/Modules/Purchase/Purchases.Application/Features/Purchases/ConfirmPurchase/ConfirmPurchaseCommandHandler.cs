using MediatR;
using Purchases.Application.Interfaces.ExternalServices;
using Purchases.Application.Interfaces.Repositories;

namespace Purchases.Application.Features.Purchases.ConfirmPurchase;

public class ConfirmPurchaseOrderCommandHandler : IRequestHandler<ConfirmPurchaseOrderCommand, PurchaseOrderConfirmationDto>
{
    private readonly IPurchasesRepository _repository;
    private readonly IInventoryHttpClient _inventoryHttpClient;

    public ConfirmPurchaseOrderCommandHandler(
        IPurchasesRepository repository, 
        IInventoryHttpClient inventoryHttpClient)
    {
        _repository = repository;
        _inventoryHttpClient = inventoryHttpClient;
    }

    public async Task<PurchaseOrderConfirmationDto> Handle(ConfirmPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _repository.GetByOrderCenForUpdateAsync(request.CompanyId, request.OrderCen, cancellationToken);

        if (purchase == null)
        {
            throw new KeyNotFoundException($"No se encontró la orden de compra con el código '{request.OrderCen}'.");
        }

        purchase.Confirm();

        var stockItems = purchase.Items.Select(item => new StockValidationItemContractDto(
            item.ProductCen,
            item.Quantity
        )).ToList();

        var inventoryPayload = new StockIncreaseContractRequest(
            WarehouseCen: purchase.WarehouseCen,
            Source: "PURCHASING_SERVICE",
            ReferenceCen: purchase.OrderCen,
            Reason: $"Ingreso de stock automatizado por confirmación de orden {purchase.OrderCen}",
            Items: stockItems
        );


        await _inventoryHttpClient.IncreaseStockAsync(request.CompanyCen, inventoryPayload);

        await _repository.UpdateAsync(purchase);

        return new PurchaseOrderConfirmationDto(
            purchase.OrderCen,
            (int)purchase.Status,
            purchase.ConfirmedAt!.Value
        );
    }
}