using MediatR;

namespace Purchase.Application.Features.Purchases.CreatePurchase;

public class CreatePurchaseCommandHandler : IRequestHandler<CreatePurchaseCommand, int>
{
    public async Task<int> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
