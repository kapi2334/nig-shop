using Microsoft.EntityFrameworkCore;
using ProduktService.Models;

namespace ProduktService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Dimensions> Dimensions { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<SurfaceType> SurfaceTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // relacja produkt → wymiary
        modelBuilder.Entity<Product>()
            .HasOne(p => p.dimensions)
            .WithMany()
            .HasForeignKey(p => p.dimensionsId);

        // relacja produkt → material
        modelBuilder.Entity<Product>()
            .HasOne(p => p.material)
            .WithMany()
            .HasForeignKey(p => p.materialId);

        // relacja produkt → typ nawierzchni
        modelBuilder.Entity<Product>()
            .HasOne(p => p.surfaceType)
            .WithMany()
            .HasForeignKey(p => p.surfaceTypeId);
    }
}
