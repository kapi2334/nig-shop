using Microsoft.EntityFrameworkCore;
using System.Net;
using UserService.Models;
using UserService.Models.Abstract;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UserService.Data
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Database variables (DbSety)
        public DbSet<Address> addresses { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Company> companies { get; set; }

        // Join table: client+company
        public DbSet<UserType> users { get; set; }

        public DbSet<CompanyEntity> companyEntities { get; set; }
        public DbSet<ClientEntity> clientEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja kluczy i relacji dla clientEntity
            modelBuilder.Entity<ClientEntity>()
                .HasKey(e => new { e.id, e.addressId });

            modelBuilder.Entity<ClientEntity>()
                .HasOne(e => e.user)
                .WithMany(u => (ICollection<ClientEntity>)u.address) // rzutowanie na ICollection
                .HasForeignKey(e => e.id);

            modelBuilder.Entity<ClientEntity>()
                .HasOne(e => e.address)
                .WithMany(a => a.clientsEntities)
                .HasForeignKey(e => e.addressId);

            // Konfiguracja kluczy i relacji dla companyEntity
            modelBuilder.Entity<CompanyEntity>()
                .HasKey(e => new { e.id, e.addressId });

            modelBuilder.Entity<CompanyEntity>()
                .HasOne(e => e.user)
                .WithMany(u => (ICollection<CompanyEntity>)u.address)
                .HasForeignKey(e => e.id);

            modelBuilder.Entity<CompanyEntity>()
                .HasOne(e => e.address)
                .WithMany(a => a.companiesEntities)
                .HasForeignKey(e => e.addressId);

            // Konfiguracja nazw tabel
            modelBuilder.Entity<UserType>().ToTable("users");
            modelBuilder.Entity<Client>().ToTable("clients");
            modelBuilder.Entity<Company>().ToTable("companies");
        }
    }
}
