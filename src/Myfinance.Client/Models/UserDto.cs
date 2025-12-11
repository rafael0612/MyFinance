namespace MyFinance.Client.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public bool IsActive { get; set; }
        public string? NameUser { get; set; }
        public string? LastName { get; set; }
        public int UserType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
