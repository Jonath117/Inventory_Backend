using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");
        
        builder.HasKey(t => t.Id);


        builder.Property(t => t.CompanyId).IsRequired();
        builder.Property(t => t.TicketNumber).HasMaxLength(20).IsRequired();
        builder.Property(t => t.WaiterName).HasMaxLength(100);
        builder.Property(t => t.CustomerName).HasMaxLength(100);
        builder.Property(t => t.CustomerPhone).HasMaxLength(20);
        

        builder.Property(t => t.SubTotal).HasPrecision(18, 2);
        builder.Property(t => t.TaxAmount).HasPrecision(18, 2);
        builder.Property(t => t.Total).HasPrecision(18, 2);
        builder.Property(t => t.TaxRate).HasPrecision(5, 2); 


        builder.HasMany(t => t.Items)
            .WithOne()
            .HasForeignKey(ti => ti.TicketId)
            .OnDelete(DeleteBehavior.Cascade); 
        
        builder.Metadata.FindNavigation(nameof(Ticket.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);


        builder.HasMany(t => t.ComandaItems)
            .WithOne()
            .HasForeignKey(ci => ci.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
               
        builder.Metadata.FindNavigation(nameof(Ticket.ComandaItems))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}