using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using System.Reflection;

namespace Sales.Infrastructure.Persistence;

public class SalesDbContext : DbContext
{
    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketItem> TicketItems { get; set; }
    public DbSet<ComandaItem> ComandaItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("sales");
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}