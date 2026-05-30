using MediatR;
using Purchases.Application.Interfaces.Repositories;
using Purchases.Domain.Entities;

namespace Purchases.Application.Features.Purchases.CreatePurchase;

public class CreatePurchaseCommandHandler : IRequestHandler<CreatePurchaseCommand, PurchaseOrderSummaryResponse>
{
    private readonly IPurchasesRepository _repository;

    public CreatePurchaseCommandHandler(IPurchasesRepository repository)
    {
        _repository = repository;
    }

    public async Task<PurchaseOrderSummaryResponse> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.SupplierCen))
            throw new ArgumentException("El proveedor es obligatorio.");
            
        if (string.IsNullOrWhiteSpace(request.WarehouseCen))
            throw new ArgumentException("La bodega destino es obligatoria.");
            
        if (request.Items == null || !request.Items.Any())
            throw new ArgumentException("La orden de compra debe contener al menos un producto.");

        var newPurchase = new Purchase(request.CompanyId, request.SupplierCen, request.WarehouseCen);

        foreach (var item in request.Items)
        {
            newPurchase.AddItem(item.ProductCen, item.Quantity);
        }

        var savedPurchase = await _repository.CreateAsync(request.CompanyId, newPurchase);

        return new PurchaseOrderSummaryResponse(
            savedPurchase.OrderCen, 
            (int)savedPurchase.Status
        );
    }
}
