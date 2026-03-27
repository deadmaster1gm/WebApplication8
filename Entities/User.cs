namespace WebApplication8.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public DateTime TimeCreated { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
