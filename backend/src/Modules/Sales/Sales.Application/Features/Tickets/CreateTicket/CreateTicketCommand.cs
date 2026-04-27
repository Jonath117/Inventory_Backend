using System.Windows.Input;
using MediatR;

namespace Sales.Application.Features.Tickets.CreateTicket;

public record CreateTicketCommand(int CompanyId) : IRequest<int>;