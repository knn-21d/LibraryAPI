namespace LibraryAPI.Data.DTOs
{
    public class AuthorDto
    {
        public string FirstName { get; set; } = null!;

        public string? Patronymic { get; set; }

        public string LastName { get; set; } = null!;
    }
}
