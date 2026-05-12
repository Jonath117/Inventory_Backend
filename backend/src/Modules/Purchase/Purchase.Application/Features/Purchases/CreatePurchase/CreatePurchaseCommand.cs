using MediatR;
using Purchase.Domain.Entities;

namespace Purchase.Application.Features.Purchases.CreatePurchase;

public record CreatePurchaseCommand(string supplier, List<PurchaseItem> Items) : IRequest<int>;