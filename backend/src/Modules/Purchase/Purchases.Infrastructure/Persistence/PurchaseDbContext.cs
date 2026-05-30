using Microsoft.EntityFrameworkCore;
using Purchases.Domain.Entities;

namespace Purchases.Infrastructure.Persistence;

public class PurchaseDbContext : DbContext
{
    public PurchaseDbContext(DbContextOptions<PurchaseDbContext> options) : base(options)
    {
    }

    public DbSet<Purchases.Domain.Entities.Purchase> Purchases { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("purchases");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PurchaseDbContext).Assembly);
    }
}