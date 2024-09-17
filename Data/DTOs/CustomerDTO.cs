namespace LibraryAPI.Data.DTOs
{
    public class CustomerDTO
    {
        public string FirstName { get; set; }
        public string? Patronymic { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string? AltPhone { get; set; }
        public string Address { get; set; }

        public CustomerDTO(string firstName, string? patronymic, string lastName, string phone, string? altPhone, string address)
        {
            FirstName = firstName;
            Patronymic = patronymic;
            LastName = lastName;
            Phone = phone;
            AltPhone = altPhone;
            Address = address;
        }
    }
}
