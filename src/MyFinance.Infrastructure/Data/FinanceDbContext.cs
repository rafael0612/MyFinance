using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.ValueObjects;
using System;
using System.Linq;

namespace MyFinance.Infrastructure.Data
{
    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
            : base(options) { }

        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Budget> Budgets { get; set; } = null!;
        public DbSet<Savings> Savings { get; set; } = null!;
        public DbSet<InternalTransfer> InternalTransfers { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Transaction → table "Transactions"
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Date)
                            .IsRequired();
                entity.Property(t => t.TransactionType)
                        .HasConversion(
                            v => v!.Name, // de TransactionType a string
                            v => TransactionType.FromName(v) // de string a TransactionType
                        )
                        .HasColumnName("TransactionType")
                        .IsRequired();

                entity.Property(t => t.Amount)
                        .HasColumnType("decimal(18,2)")
                        .IsRequired();

                entity.Property(t => t.Description)
                            .HasMaxLength(500)
                            .IsRequired(false); // Puede ser nulo

                entity.Property(t => t.TipoIngreso)
                      .HasMaxLength(100) // Limitar el tamaño del campo
                      .IsRequired(); // Campo obligatorio

                entity.Property(t => t.OrigenIngreso)
                      .HasMaxLength(100) // Limitar el tamaño del campo
                      .IsRequired(); // Campo obligatorio

                entity.Property(t => t.ExpenseCategory)
                      .HasMaxLength(100) // Limitar el tamaño del campo
                      .IsRequired(false); // Puede ser nulo

                entity.Property(t => t.NivelNecesidad)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(t => t.NaturalezaGasto)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(t => t.EsFijo)
                      .IsRequired();
                entity.Property(t => t.UserId)
                      .IsRequired();

                entity.HasOne(t => t.User)
                      .WithMany()
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
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
                entity.HasOne(t => t.User)
                      .WithMany()
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Savings → table "Savings"
            modelBuilder.Entity<Savings>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name)
                      .HasMaxLength(200)
                      .IsRequired();
                entity.Property(s => s.Balance)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();
            });

            // InternalTransfer → table "InternalTransfers"
            modelBuilder.Entity<InternalTransfer>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.FromAccountId)
                      .IsRequired();
                entity.Property(t => t.ToAccountId)
                      .IsRequired();
                entity.Property(t => t.Amount)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();
                entity.Property(t => t.Date)
                      .IsRequired();
            });

            // User → table "Users"
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email)
                      .HasMaxLength(200)
                      .IsRequired();
                entity.Property(u => u.PasswordHash)
                      .HasMaxLength(200)
                      .IsRequired();
                entity.HasIndex(u => u.Email)
                      .IsUnique(); // Para evitar correos duplicados
                entity.Property(u => u.IsActive)
                      .IsRequired();
                entity.Property(u => u.NameUser)
                      .HasMaxLength(100);           // nullable por defecto

                entity.Property(u => u.LastName)
                      .HasMaxLength(100);           // nullable por defecto

                entity.Property(u => u.FullName)
                      .HasMaxLength(200);           // nullable por defecto

                entity.Property(u => u.UserType)
                      .HasConversion<int>() // Guardar el enum como int
                      .IsRequired();

                entity.Property(u => u.CreatedAt)
                      .HasColumnType("datetime2")
                      .HasDefaultValueSql("DATEADD(HOUR, -5, SYSUTCDATETIME())") // o GETUTCDATE()
                      .ValueGeneratedOnAdd();
            });

            base.OnModelCreating(modelBuilder);
        }

        public IQueryable<Transaction> GetTransactionsByPeriod(DateTime period)
        {
            return Transactions.Where(t => t.Date.Year == period.Year && t.Date.Month == period.Month);
        }
    }
}