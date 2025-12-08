using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MyFinance.Infrastructure.Data;

namespace MyFinance.Infrastructure
{
    public class FinanceDbContextFactory : IDesignTimeDbContextFactory<FinanceDbContext>
    {
        public FinanceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder.UseSqlServer("TU-CONEXION-DB");

            return new FinanceDbContext(optionsBuilder.Options);
        }
    }
}