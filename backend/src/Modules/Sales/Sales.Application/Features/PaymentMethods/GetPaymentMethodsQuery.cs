using MediatR;
using Sales.Application.Features.Tickets;

namespace Sales.Application.Features.PaymentMethods;

public record GetPaymentMethodsQuery() : IRequest<IEnumerable<PaymentMethodContractResponse>>;

public record PaymentMethodContractResponse(
    int Id,
    string Name,
    string Code,
    bool IsActive
);

public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, IEnumerable<PaymentMethodContractResponse>>
{
    public async Task<IEnumerable<PaymentMethodContractResponse>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        return new List<PaymentMethodContractResponse>
        {
            new PaymentMethodContractResponse(1, "Efectivo", "CASH", true),
            new PaymentMethodContractResponse(2, "QR", "QR", true),
            new PaymentMethodContractResponse(3, "Tarjeta", "CARD", true)
        };
    }
}