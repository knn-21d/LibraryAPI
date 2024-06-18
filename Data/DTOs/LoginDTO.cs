using System.Text.Json.Serialization;

namespace LibraryAPI.Data.DTOs
{
    public class LoginDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }

        [JsonConstructor]
        public LoginDTO(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
