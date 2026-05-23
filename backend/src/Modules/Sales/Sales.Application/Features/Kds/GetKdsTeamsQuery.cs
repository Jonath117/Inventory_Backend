using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;

namespace Sales.Application.Features.Kds;

public record GetKdsTeamsQuery(int CompanyId) : IRequest<IEnumerable<KdsTeamContractResponse>>;

public class GetKdsTeamsQueryHandler : IRequestHandler<GetKdsTeamsQuery, IEnumerable<KdsTeamContractResponse>>
{
    public async Task<IEnumerable<KdsTeamContractResponse>> Handle(GetKdsTeamsQuery request, CancellationToken cancellationToken)
    {
        return new List<KdsTeamContractResponse>
        {
            new KdsTeamContractResponse("T-001", "Cocina Principal"),
            new KdsTeamContractResponse("T-002", "Bar")
        };
    }
}