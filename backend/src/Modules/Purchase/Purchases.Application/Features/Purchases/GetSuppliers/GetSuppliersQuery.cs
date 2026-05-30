using MediatR;

namespace Purchases.Application.Features.Purchases.GetSuppliers;

public record SupplierDto(string SupplierCen, string Name);

public record GetSuppliersQuery(int CompanyId) : IRequest<List<SupplierDto>>;