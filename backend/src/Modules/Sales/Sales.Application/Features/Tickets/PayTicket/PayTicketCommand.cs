using MediatR;
using Sales.Application.Interfaces;
using Sales.Domain.Enums;
using Sales.Application.Features.Tickets;
using Sales.Application.Interfaces.ExternalServices;

namespace Sales.Application.Features.Tickets;

public record PayTicketCommand(int CompanyId, string CompanyCen, string TicketCen, PayTicketContractRequest Request) : IRequest<PayTicketContractResponse>;

public class PayTicketCommandHandler : IRequestHandler<PayTicketCommand, PayTicketContractResponse>
{
    private readonly ISalesRepository _repository;
    private readonly IInventoryHttpClient _inventoryClient;

    public PayTicketCommandHandler(ISalesRepository repository, IInventoryHttpClient inventoryClient)
    {
        _repository = repository;
        _inventoryClient = inventoryClient;
    }

    public async Task<PayTicketContractResponse> Handle(PayTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByCenAsync(request.CompanyId, request.TicketCen, cancellationToken);
        if (ticket == null)
        {
            throw new KeyNotFoundException($"El ticket '{request.TicketCen}' no existe.");
        }

        var consumeRequest = new StockConsumeRequestDto(
            WarehouseCen: ticket.WarehouseCen,
            Source: "SALES_MODULE",
            ReferenceCen: ticket.TicketCen,
            Reason: $"Venta ticket {ticket.TicketNumber}",
            Items: ticket.Items.Select(i => new StockConsumeItemDto(i.ProductCen, i.Quantity)).ToList()
        );

        var consumeResult = await _inventoryClient.ConsumeStockAsync(request.CompanyCen, consumeRequest);

        if (!consumeResult.Success)
        {
            string missingDetails = "Revisa el inventario.";
            if (consumeResult.Requirements != null && consumeResult.Requirements.Any())
            {
                var missingItems = consumeResult.Requirements
                    .Select(r => $"{r.ProductName} (Faltan {r.MissingQuantity})");
                missingDetails = string.Join(", ", missingItems);
            }
            throw new InvalidOperationException($"No se pudo cobrar. Stock insuficiente para: {missingDetails}");
        }

        ticket.Pay((PaymentMethod)request.Request.PaymentMethodId);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new PayTicketContractResponse(
            TicketCen: ticket.TicketCen,
            Status: ticket.Status.ToString(),
            PaidAt: ticket.PaidAt ?? DateTime.UtcNow
        );
    }
}