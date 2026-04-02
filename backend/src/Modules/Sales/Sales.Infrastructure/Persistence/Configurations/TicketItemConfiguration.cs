using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Persistence.Configurations;

public class TicketItemConfiguration : IEntityTypeConfiguration<TicketItem>
{
    public void Configure(EntityTypeBuilder<TicketItem> builder)
    {
        builder.ToTable("ticket_items");

        builder.HasKey(ti => ti.Id);

        builder.Property(ti => ti.ProductName).HasMaxLength(150).IsRequired();
        builder.Property(ti => ti.Note).HasMaxLength(250);
        
        builder.Property(ti => ti.Quantity).HasPrecision(18, 2);
        builder.Property(ti => ti.UnitPrice).HasPrecision(18, 2);
        
        builder.Ignore(ti => ti.SubTotal);
    }
}