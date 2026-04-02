using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Persistence.Configurations;

public class ComandaItemConfiguration : IEntityTypeConfiguration<ComandaItem>
{
    public void Configure(EntityTypeBuilder<ComandaItem> builder)
    {
        builder.ToTable("comanda_items");

        builder.HasKey(ci => ci.Id);
        
        builder.Property(ci => ci.Station).IsRequired();
        builder.Property(ci => ci.Status).IsRequired();
    }
}