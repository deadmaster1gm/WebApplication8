namespace WebApplication8.Contracts.DTO
{
    public class UpdateUserRequestDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
