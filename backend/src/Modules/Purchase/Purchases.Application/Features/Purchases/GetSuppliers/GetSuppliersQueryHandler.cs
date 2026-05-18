using MediatR;
using Purchases.Application.Interfaces.Repositories;

namespace Purchases.Application.Features.Purchases.GetSuppliers;

public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, List<SupplierDto>>
{
    private readonly IPurchasesRepository _repository;

    public GetSuppliersQueryHandler(IPurchasesRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SupplierDto>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await _repository.GetSuppliersAsync(request.CompanyId, cancellationToken);

        return suppliers.Select(s => new SupplierDto(
            s.SupplierCen,
            s.Name
        )).ToList();
    }
}