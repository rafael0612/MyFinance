namespace MyFinance.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

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
        }
    }
}
