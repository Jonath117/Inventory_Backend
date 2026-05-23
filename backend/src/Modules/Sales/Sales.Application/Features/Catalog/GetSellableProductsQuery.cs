using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;
using Sales.Application.Interfaces.ExternalServices;

namespace Sales.Application.Features.Catalog;

public record GetSellableProductsQuery(int CompanyId, string CompanyCen, string? Search, string? CategoryCen, string? WarehouseCen, bool OnlyAvailable, int Page, int PageSize) : IRequest<IEnumerable<SellableProductContractDto>>;

public class GetSellableProductsQueryHandler : IRequestHandler<GetSellableProductsQuery, IEnumerable<SellableProductContractDto>>
{
    private readonly IInventoryHttpClient _inventoryClient;

    public GetSellableProductsQueryHandler(IInventoryHttpClient inventoryClient)
    {
        _inventoryClient = inventoryClient;
    }

    public async Task<IEnumerable<SellableProductContractDto>> Handle(GetSellableProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _inventoryClient.GetSellableProductsAsync(
            request.CompanyCen, 
            request.Search, 
            request.CategoryCen, 
            request.WarehouseCen, 
            request.OnlyAvailable, 
            request.Page, 
            request.PageSize
        );

        return products.Select(p => new SellableProductContractDto(
            p.ProductCen,
            p.Sku,
            p.Name,
            p.Description,
            p.CategoryName,
            p.UnitName,
            p.SalePrice,
            p.AvailableStock,
            p.IsAvailable
        ));
    }
}