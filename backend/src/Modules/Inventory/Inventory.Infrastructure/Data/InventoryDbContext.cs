using Microsoft.EntityFrameworkCore;
using Inventory.Domain.Entities;
using Shared.Domain;

namespace Inventory.Infrastructure.Data;

public class InventoryDbContext : DbContext 
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
    }
    
    public DbSet<Company> Companies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<InventoryStock> InventoryStocks { get; set; }
    public DbSet<InventoryMovement> InventoryMovements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("inventory");
        
        // Mapeos básicos
        modelBuilder.Entity<Company>().ToTable("companies", "core");
        modelBuilder.Entity<Warehouse>().ToTable("warehouses");
        modelBuilder.Entity<InventoryStock>().ToTable("inventory_stock");
        modelBuilder.Entity<InventoryMovement>().ToTable("inventory_movements");
        
        // Categorías
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });
        
        // Unidades
        modelBuilder.Entity<Unit>(entity =>
        {
            entity.ToTable("units");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            
            // Regla: No se pueden repetir nombres de unidades en la misma empresa
            entity.HasIndex(e => new { e.CompanyId, e.Name }).IsUnique();
        });
        
        // Productos
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            
            // Regla: Precio decimal preciso
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.SalePrice).HasColumnType("decimal(18,2)");

            // Índices Únicos
            entity.HasIndex(p => new { p.CompanyId, p.Sku }).IsUnique();

            // Relaciones
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); 

            entity.HasOne(p => p.Unit)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.Restrict); 
        });

        // Stock único por almacen mas producto
        modelBuilder.Entity<InventoryStock>()
            .HasIndex(s => new { s.WarehouseId, s.ProductId })
            .IsUnique();
        
        // Restricción de borrado para movimientos
        modelBuilder.Entity<InventoryMovement>()
            .HasOne(m => m.Product)
            .WithMany()
            .HasForeignKey(m => m.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}