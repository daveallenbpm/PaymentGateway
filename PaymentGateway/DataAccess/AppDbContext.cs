using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<PaymentEntity> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PaymentEntity>()
                .HasIndex(p => p.PaymentId)
                .IsUnique();

            modelBuilder
                .Entity<PaymentEntity>()
                .Property(p => p.CVV)
                .HasMaxLength(3);

            modelBuilder
                .Entity<PaymentEntity>()
                .Property(p => p.Currency)
                .HasMaxLength(3);
        }
    }
}
