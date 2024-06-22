namespace LibraryAPI.Data.DTOs
{
    public class CustomerJwt
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string? Patronymic { get; set; }

        public string LastName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? AltPhone { get; set; }

        public string Address { get; set; } = null!;

        public int UserId { get; set; }
    }
}
