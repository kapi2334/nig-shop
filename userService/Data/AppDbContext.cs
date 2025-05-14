using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> addresses { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Client> clients { get; set; }

        public DbSet<CompanyEntity> companyEntities { get; set; }
        public DbSet<ClientEntity> clientEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientEntity>()
                .HasKey(fa => new { fa.id, fa.addressId });

            modelBuilder.Entity<ClientEntity>()
                    .HasOne(oa => oa.user)
                    .WithMany(c => (List<ClientEntity>)c.address)
                    .HasForeignKey(oa => oa.id);

            modelBuilder.Entity<ClientEntity>()
                .HasOne(oa => oa.address)
                .WithMany(a => a.clientsEntities)
                .HasForeignKey(oa => oa.addressId);

            modelBuilder.Entity<CompanyEntity>()
                .HasKey(fa => new { fa.id, fa.addressId });

            modelBuilder.Entity<CompanyEntity>()
                    .HasOne(oa => oa.user)
                    .WithMany(c => (List<CompanyEntity>)c.address)
                    .HasForeignKey(oa => oa.id);

            modelBuilder.Entity<CompanyEntity>()
                .HasOne(oa => oa.address)
                .WithMany(a => a.companiesEntities)
                .HasForeignKey(oa => oa.addressId);

        }
    }
}
