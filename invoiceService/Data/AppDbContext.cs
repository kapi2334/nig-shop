using Microsoft.EntityFrameworkCore;
using InvoiceService.Models;

namespace InvoiceService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Issuer> issuer { get; set; }
        public DbSet<Invoice> invoice { get; set; }
        public DbSet<ProductInfo> productInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issuer>().ToTable("wystawca");
            modelBuilder.Entity<Invoice>().ToTable("faktura");
            modelBuilder.Entity<ProductInfo>().ToTable("produktinfo");

            // Relacja Issuer -> wiele Invoice
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.issuer)
                .WithMany(w => w.invoices)
                .HasForeignKey(i => i.issuerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja Invoice -> wiele ProductInfo
            modelBuilder.Entity<ProductInfo>()
                .HasOne<Invoice>()
                .WithMany(i => i.products)
                .HasForeignKey(p => p.invoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
