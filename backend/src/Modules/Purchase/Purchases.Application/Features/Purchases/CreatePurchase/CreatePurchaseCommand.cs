using MediatR;
using Purchases.Domain.Entities;

namespace Purchases.Application.Features.Purchases.CreatePurchase;

public record CreatePurchaseCommand(string supplier, List<PurchaseItem> Items) : IRequest<int>;