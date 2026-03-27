using WebApplication8.Entities;

namespace WebApplication8.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(CancellationToken ct);
        Task<User?> GetByIdAsync (Guid id, CancellationToken ct);
        Task AddAsync (User user, CancellationToken ct);
        Task UpdateAsync (User user, CancellationToken ct);
        Task DeleteAsync (User user, CancellationToken ct);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task<bool> ExistsByEmailAsync(string email, Guid id, CancellationToken ct)  ;
    }
}
