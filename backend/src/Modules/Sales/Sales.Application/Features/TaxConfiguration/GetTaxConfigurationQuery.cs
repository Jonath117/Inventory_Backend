using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;

namespace Sales.Application.Features.TaxConfiguration;

public record GetTaxConfigurationQuery(int CompanyId) : IRequest<TaxConfigurationContractResponse>;

public class GetTaxConfigurationQueryHandler : IRequestHandler<GetTaxConfigurationQuery, TaxConfigurationContractResponse>
{
    public async Task<TaxConfigurationContractResponse> Handle(GetTaxConfigurationQuery request, CancellationToken cancellationToken)
    {
        return new TaxConfigurationContractResponse(13.00m);
    }
}