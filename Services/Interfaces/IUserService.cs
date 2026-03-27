using WebApplication8.Contracts.DTO;

namespace WebApplication8.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct);
        Task<UserDto> GetByIdAsync(Guid id, CancellationToken ct);
        Task<UserDto> CreateAsync(CreateUserRequestDto dto, CancellationToken ct);
        Task<bool> UpdateAsync(Guid id, UpdateUserRequestDto dto, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    }
}
