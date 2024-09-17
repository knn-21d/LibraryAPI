using System.Text.Json.Serialization;

namespace LibraryAPI.Data.DTOs
{
    public class RegisterDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public CustomerDTO CustomerData { get; set; }

        [JsonConstructor]
        public RegisterDTO(string login, string password, CustomerDTO customerData)
        {
            Login = login;
            Password = password;
            CustomerData = customerData;
        }
    }
}
