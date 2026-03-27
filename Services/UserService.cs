using WebApplication8.Contracts.DTO;
using WebApplication8.Entities;
using WebApplication8.Exceptions;
using WebApplication8.Repositories.Interfaces;
using WebApplication8.Services.Interfaces;

namespace WebApplication8.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct)
        {
            var users = await _repository.GetAllAsync(ct);

            return users
                .Select(MapToDto)
                .ToList();
        }
        public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var user = await _repository.GetByIdAsync(id, ct);

            if (user == null)
                return null;

            return MapToDto(user);
        }
        public async Task<UserDto> CreateAsync(CreateUserRequestDto dto, CancellationToken ct)
        {
            var normalizedEmail = dto.Email.Trim().ToLowerInvariant();

            var exists = await _repository.ExistsByEmailAsync(normalizedEmail, ct);

            if (exists)
            {
                throw new EmailAlreadyExistsException(normalizedEmail);
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                TimeCreated = DateTime.UtcNow,
                OrderNumber = $"ORD-{Random.Shared.Next(1000, 10000)}",
                Name = dto.Name,
                Surname = dto.Surname,
                Email = normalizedEmail
            };

            await _repository.AddAsync(user, ct);

            return MapToDto(user);
        }
        public async Task<bool> UpdateAsync(Guid id, UpdateUserRequestDto dto, CancellationToken ct)
        {
            var user = await _repository.GetByIdAsync(id, ct);

            if (user == null)
                return false;

            var normalizedEmail = dto.Email.Trim().ToLowerInvariant();

            var exists = await _repository.ExistsByEmailAsync(normalizedEmail, id, ct);

            if(exists)
            {
                throw new EmailAlreadyExistsException(normalizedEmail);
            }

            user.Name = dto.Name;
            user.Surname = dto.Surname; 
            user.Email = normalizedEmail;

            await _repository.UpdateAsync(user, ct);
            return true;
        }
        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var user = await _repository.GetByIdAsync(id, ct);

            if (user == null)
                return false;

            await _repository.DeleteAsync(user, ct);
            return true;
        }

        private static UserDto MapToDto (User user)
        {
            return new UserDto
            {
                Id = user.Id,
                TimeCreated = user.TimeCreated,
                OrderNumber = user.OrderNumber,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email
            };
        }
    }
}
