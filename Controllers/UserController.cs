using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WebApplication8.Contracts.DTO;
using WebApplication8.Services.Interfaces;

namespace WebApplication8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers(CancellationToken ct)
        {
            var users = await _service.GetAllAsync(ct);
            return Ok(users);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id, CancellationToken ct)
        {
            var user = await _service.GetByIdAsync(id, ct);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [HttpPost]
        public async Task <ActionResult<UserDto>> CreateUser (CreateUserRequestDto dto,  CancellationToken ct)
        {
            var user = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetUserById), new { Id = user.Id }, user);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser (Guid id, UpdateUserRequestDto dto, CancellationToken ct)
        {
            var user = await _service.UpdateAsync(id, dto, ct);
                if (!user)
                    return NotFound();
            return NoContent();
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken ct)
        {
            var user = await _service.DeleteAsync(id, ct);
            if (!user)
                return NotFound();
            return NoContent();
        }
    }
}
