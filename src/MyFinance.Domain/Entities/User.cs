namespace MyFinance.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public bool IsActive { get; private set; } = true;
        public string? NameUser { get; private set; } = string.Empty; // NUEVO
        public string? LastName { get; private set; } = string.Empty; // NUEVO
        public string? FullName { get; private set; } = string.Empty; // NUEVO
        public DateTime CreatedAt { get; private set; }              // NUEVO

        private User() { }

        public User(string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo es obligatorio.");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("La contraseña es obligatoria.");

            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            IsActive = true;
            CreatedAt = DateTime.UtcNow.AddHours(-5);
        }
        public User(string email, string passwordHash, string? nameUser = null, string? lastName = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo es obligatorio.");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("La contraseña es obligatoria.");

            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            IsActive = true;
            NameUser = nameUser;
            LastName = lastName;
            FullName = $"{nameUser} {lastName}".Trim();
            CreatedAt = DateTime.UtcNow.AddHours(-5);
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
