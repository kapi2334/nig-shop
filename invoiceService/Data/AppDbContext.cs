using Microsoft.EntityFrameworkCore;
using InvoiceService.Models;

namespace InvoiceService.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Issuer> issuer { get; set; }
    public DbSet<Invoice> invoice { get; set; }
    public DbSet<Models.ProductInfo> productInfo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Issuer>().ToTable("wystawca");
        modelBuilder.Entity<Invoice>().ToTable("faktura");
        modelBuilder.Entity<Models.ProductInfo>().ToTable("produktinfo");
    }
}
