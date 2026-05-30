using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;

namespace Sales.Application.Features.Waiters;

public record GetWaitersQuery(int CompanyId) : IRequest<IEnumerable<WaiterContractResponse>>;

public class GetWaitersQueryHandler : IRequestHandler<GetWaitersQuery, IEnumerable<WaiterContractResponse>>
{
    public async Task<IEnumerable<WaiterContractResponse>> Handle(GetWaitersQuery request, CancellationToken cancellationToken)
    {
        return new List<WaiterContractResponse>
        {
            new WaiterContractResponse("W-001", "Juan Perez", true),
            new WaiterContractResponse("W-002", "Maria Garcia", true),
            new WaiterContractResponse("W-003", "Matias Molina", true)
        };
    }
}