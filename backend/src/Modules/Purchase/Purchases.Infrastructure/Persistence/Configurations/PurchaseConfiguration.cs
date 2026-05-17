using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Purchases.Infrastructure.Persistence.Configurations;

public class PurchaseConfiguration : IEntityTypeConfiguration<Purchases.Domain.Entities.Purchase>
{
    public void Configure(EntityTypeBuilder<Purchases.Domain.Entities.Purchase> builder)
    {
        builder.ToTable("purchases");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.OrderCen)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.SupplierCen)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<int>();
        
        builder.HasIndex(p => p.CompanyId);
        
        builder.HasIndex(p => new { p.CompanyId, p.OrderCen })
            .IsUnique();
        
        builder.HasIndex(p => p.SupplierCen);
    }
}