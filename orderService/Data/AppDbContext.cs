using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderedProducts> OrderedProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("zamowienie");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();
            });

            // Configure OrderedProducts entity
            modelBuilder.Entity<OrderedProducts>(entity =>
            {
                entity.ToTable("zamowienie_produkty");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                // Configure foreign key relationship
                entity.HasOne<Order>()
                    .WithMany(o => o.products)
                    .HasForeignKey(op => op.orderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
