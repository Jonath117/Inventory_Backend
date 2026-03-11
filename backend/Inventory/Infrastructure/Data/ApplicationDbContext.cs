using Microsoft.EntityFrameworkCore;

using WebApp1.Domain.Entities;

namespace WebApp1.Infrastructure.Data;

public class ApplicationDbContext : DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Company> Companies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<InventoryStock> InventoryStocks { get; set; }
    public DbSet<InventoryMovement> InventoryMovements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Company>().ToTable("companies");
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Warehouse>().ToTable("warehouses");
        modelBuilder.Entity<InventoryStock>().ToTable("inventory_stock");
        modelBuilder.Entity<InventoryMovement>().ToTable("inventory_movements");
        
        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.CompanyId, p.Sku })
            .IsUnique();
        
        //stock unico por almacen mas producto
        modelBuilder.Entity<InventoryStock>()
            .HasIndex(s => new { s.WarehouseId, s.ProductId })
            .IsUnique();
        
        modelBuilder.Entity<InventoryMovement>()
            .HasOne(m => m.Product)
            .WithMany()
            .HasForeignKey(m => m.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}