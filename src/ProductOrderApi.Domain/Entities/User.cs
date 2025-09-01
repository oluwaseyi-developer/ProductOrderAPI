namespace ProductOrderApi.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PasswordHash { get; private set; }
        public ICollection<string> Roles { get; private set; } = new List<string>();

        protected User() { }

        public User(string email, string firstName, string lastName, string passwordHash)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        }

        public void AddRole(string role)
        {
            if (!Roles.Contains(role))
                Roles.Add(role);
        }

        public void RemoveRole(string role)
        {
            Roles.Remove(role);
        }

        public bool IsInRole(string role) => Roles.Contains(role);
    }
}
