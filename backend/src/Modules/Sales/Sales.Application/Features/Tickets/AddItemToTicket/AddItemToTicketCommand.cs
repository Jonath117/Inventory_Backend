using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;
using Sales.Application.Interfaces.ExternalServices;

namespace Sales.Application.Features.Tickets;

public record AddItemToTicketCommand(int CompanyId, string CompanyCen, string TicketCen, CreateTicketItemContractRequest Item) : IRequest<TicketItemContractResponse?>;

public class AddItemToTicketCommandHandler : IRequestHandler<AddItemToTicketCommand, TicketItemContractResponse?>
{
    private readonly ISalesRepository _repository;
    private readonly IInventoryHttpClient _inventoryClient;

    public AddItemToTicketCommandHandler(ISalesRepository repository, IInventoryHttpClient inventoryClient)
    {
        _repository = repository;
        _inventoryClient = inventoryClient;
    }

    public async Task<TicketItemContractResponse?> Handle(AddItemToTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByCenAsync(request.CompanyId, request.TicketCen, cancellationToken);
        if (ticket == null) return null;

        var productDetails = await _inventoryClient.GetProductDetailsAsync(request.CompanyCen, request.Item.ProductCen);

        if (productDetails == null || !productDetails.IsAvailable)
        {
            throw new InvalidOperationException(
                $"El producto con CEN {request.Item.ProductCen} no existe o no está disponible para la venta.");
        }

        ticket.AddOrUpdateItem(
            request.Item.ProductCen,
            productDetails.Name,
            request.Item.Quantity,
            productDetails.SalePrice,
            request.Item.Note
        );

        await _repository.SaveChangesAsync(cancellationToken);

        var item = ticket.Items.First(i => i.ProductCen == request.Item.ProductCen);

        return new TicketItemContractResponse(
            TicketItemCen: item.TicketItemCen,
            ProductCen: item.ProductCen,
            ProductName: item.ProductName,
            Quantity: (int)item.Quantity,
            UnitPrice: item.UnitPrice,
            SubTotal: item.SubTotal,
            Note: item.Note,
            Status: "Added"
        );
    }
}