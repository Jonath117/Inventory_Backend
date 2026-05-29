using MediatR;
using Sales.Application.Interfaces;
using Sales.Domain.Enums;
using Sales.Application.Features.Tickets;
using Sales.Application.Interfaces.ExternalServices;
using Sales.Domain.Exceptions;

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
            throw new NotFoundException("Ticket", request.TicketCen);
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
            throw new BadRequestException("No se pudo completar la venta por falta de stock en Inventario.");
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