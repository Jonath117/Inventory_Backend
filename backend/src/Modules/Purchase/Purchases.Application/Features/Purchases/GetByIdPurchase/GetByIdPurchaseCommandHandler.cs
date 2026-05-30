using MediatR;
using Purchases.Application.Interfaces.Repositories;

namespace Purchases.Application.Features.Purchases.GetByIdPurchase;

public class GetPurchaseOrderDetailQueryHandler : IRequestHandler<GetPurchaseOrderDetailQuery, PurchaseOrderDetailDto>
{
    private readonly IPurchasesRepository _repository;

    public GetPurchaseOrderDetailQueryHandler(IPurchasesRepository repository)
    {
        _repository = repository;
    }

    public async Task<PurchaseOrderDetailDto> Handle(GetPurchaseOrderDetailQuery request, CancellationToken cancellationToken)
    {
        var purchase = await _repository.GetByOrderCenAsync(request.CompanyId, request.OrderCen, cancellationToken);

        if (purchase == null)
        {
            throw new KeyNotFoundException($"La orden de compra con código '{request.OrderCen}' no existe.");
        }

        var itemsDto = purchase.Items.Select(item => new PurchaseOrderDetailItemDto(
            item.ProductCen,
            item.Quantity
        )).ToList();

        return new PurchaseOrderDetailDto(
            purchase.OrderCen,
            (int)purchase.Status,
            purchase.CreatedAt,
            purchase.ConfirmedAt,
            purchase.SupplierCen,
            purchase.WarehouseCen,
            itemsDto
        );
    }
}