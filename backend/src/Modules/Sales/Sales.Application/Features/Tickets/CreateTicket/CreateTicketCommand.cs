using MediatR;

namespace Sales.Application.Features.Tickets;

public record CreateTicketCommand(int CompanyId, string? WaiterCen, string WarehouseCen) : IRequest<TicketContractResponse>;