namespace ErsaCommerce.Domain
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User"; // "Admin" olabilir

    }
}
