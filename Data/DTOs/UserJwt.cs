namespace LibraryAPI.Data.DTOs
{
    public class UserJwt
    {
        public int Id { get; set; }

        public string Login { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
