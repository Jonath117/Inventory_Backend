using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchases.Domain.Entities;

namespace Purchases.Infrastructure.Persistence.Configurations;

public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable("purchases", "purchases");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.OrderCen).IsRequired().HasMaxLength(150);
        builder.Property(e => e.SupplierCen).IsRequired().HasMaxLength(150);
        builder.Property(e => e.WarehouseCen).IsRequired().HasMaxLength(150);

        builder.HasIndex(e => new { e.CompanyId, e.OrderCen }).IsUnique();

        var navigation = builder.Metadata.FindNavigation(nameof(Purchase.Items));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}