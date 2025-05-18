using Microsoft.EntityFrameworkCore;
using InvoiceService.Models;

namespace InvoiceService.Data
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Issuer> Issuer { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issuer>().ToTable("wystawca");
            modelBuilder.Entity<Invoice>().ToTable("faktura");
            modelBuilder.Entity<Product>().ToTable("produkt");

            // Invoice ma odniesienie do Issuer, ale Issuer nie ma do Invoice
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.issuer)
                .WithMany()
                .HasForeignKey(i => i.issuerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invoice ma odniesienie do Product, ale Product nie ma do Invoice
            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.products)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
