using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure.Data;

namespace MyFinance.Infrastructure.Repositories
{
    public class EFCoreUserRepository : IUserRepository
    {
        private readonly FinanceDbContext _context;
        public EFCoreUserRepository(FinanceDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
