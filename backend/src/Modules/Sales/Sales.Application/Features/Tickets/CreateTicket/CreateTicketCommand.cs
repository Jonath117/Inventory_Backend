using MediatR;

namespace Sales.Application.Features.Tickets.CreateTicket;

public record CreateTicketCommand(int CompanyId, string? WaiterCen, string WarehouseCen) : IRequest<TicketContractResponse>;