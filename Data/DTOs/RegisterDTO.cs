using System.Text.Json.Serialization;

namespace LibraryAPI.Data.DTOs
{
    public class RegisterDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string[] CustomerData { get; set; }

        [JsonConstructor]
        public RegisterDTO(string login, string password, string[] customerData)
        {
            Login = login;
            Password = password;
            CustomerData = customerData;
        }
    }
}
