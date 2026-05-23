using MediatR;
using Sales.Application.Interfaces;
using Sales.Application.Features.Tickets;

namespace Sales.Application.Features.TaxConfiguration.UpdateTaxConfiguration;

public record UpdateTaxConfigurationCommand(int CompanyId, UpdateTaxConfigurationContractRequest Request) : IRequest<TaxConfigurationContractResponse>;

public class UpdateTaxConfigurationCommandHandler : IRequestHandler<UpdateTaxConfigurationCommand, TaxConfigurationContractResponse>
{
    public async Task<TaxConfigurationContractResponse> Handle(UpdateTaxConfigurationCommand request, CancellationToken cancellationToken)
    {
        return new TaxConfigurationContractResponse(request.Request.TaxRate);
    }
}