using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.ValueObjects;

namespace MyFinance.Infrastructure.Data
{
    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
            : base(options) { }

        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Budget>      Budgets      { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Transaction → table "Transactions"
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Date)
                      .IsRequired();

                // Category como ValueObject
                entity.OwnsOne(t => t.Category, cb =>
                {
                    cb.Property(c => c.Name)
                      .HasColumnName("Category")
                      .IsRequired();
                });

                entity.Property(t => t.Amount)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();
            });

            // Budget → table "Budgets"
            modelBuilder.Entity<Budget>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Year)
                      .IsRequired();
                entity.Property(b => b.Month)
                      .IsRequired();
                entity.Property(b => b.Amount)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();
                entity.Property(b => b.AlertThreshold)
                      .HasColumnType("decimal(5,4)") // p.ej. 0.8000
                      .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

