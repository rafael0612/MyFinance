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
        public UserType UserType { get; private set; } // NUEVO
        public DateTime CreatedAt { get; private set; } // NUEVO

        private User() { }

        public User(string email, string passwordHash, UserType userType = UserType.Standard)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo es obligatorio.");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("La contraseña es obligatoria.");

            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            IsActive = true;
            UserType = userType;
            CreatedAt = DateTime.UtcNow.AddHours(-5);
        }
        public User(string email, string passwordHash, string? nameUser = null, string? lastName = null, UserType userType = UserType.Standard)
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
            UserType = userType;
            FullName = $"{nameUser} {lastName}".Trim();
            CreatedAt = DateTime.UtcNow.AddHours(-5);
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
        public void ChangeNameUser(string? newNameUser) => NameUser = newNameUser ?? throw new ArgumentNullException(nameof(newNameUser));
        public void ChangeLastName(string? newLastName) => LastName = newLastName ?? throw new ArgumentNullException(nameof(newLastName));
        public void ChangeFullName() => FullName = $"{NameUser} {LastName}".Trim();
        public void ChangeIsActive(bool newIsActive) => IsActive = newIsActive;
        public void ChangeUserType(UserType newUserType) => UserType = newUserType;
    }
    public enum UserType
    {
        Standard = 0,
        Admin = 1
    }
}
