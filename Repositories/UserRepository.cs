using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Entities;
using WebApplication8.Repositories.Interfaces;

namespace WebApplication8.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Users.ToListAsync(ct);
        }
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Users.FindAsync(id, ct);
        }
        public async Task AddAsync(User user, CancellationToken ct)
        {
            await _context.Users.AddAsync(user, ct);
            await _context.SaveChangesAsync(ct);
        }
        public async Task UpdateAsync(User user, CancellationToken ct)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(ct);
        }
        public async Task DeleteAsync(User user, CancellationToken ct)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);
        }
        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
        {
            return await _context.Users.AnyAsync(u => u.Email == email, ct);

        }
        public async Task<bool> ExistsByEmailAsync(string email, Guid id, CancellationToken ct)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.Id == id, ct)   ;
        }
    }
}
