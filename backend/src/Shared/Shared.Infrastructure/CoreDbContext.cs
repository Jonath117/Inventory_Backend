namespace Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Domain;

public class CoreDbContext : DbContext
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options){ }
    
    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("core");
        
        modelBuilder.Entity<Company>().ToTable("companies");
    }
}