using LibraryAPI.Data.Repositories;
using LibraryAPI.Models;
using System.Web.Helpers;
using System.Web.Http;

namespace LibraryAPI.Services
{
    public class UserManagementService
    {
        private readonly UsersRepository _usersRepository;
        private readonly CustomersRepository _customersRepository;

        public UserManagementService(UsersRepository usersRepository, CustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
            _usersRepository = usersRepository;
        }

        public async Task<User> RegisterUser(string login, string password, int roleId)
        {
            return await _usersRepository.AddUser(new User { Login = login, PasswordHash = Crypto.HashPassword(password), RoleId = roleId});
        }

        public async Task<User> GetUser(int id)
        {
            return await _usersRepository.GetUser(id) ?? throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _usersRepository.GetAllUsers();
        }

        public async Task<User> EditUser(int id, User user)
        {
            return await _usersRepository.EditUser(id, user);
        }

        public async Task<User> DeleteUser(int id)
        {
            return await _usersRepository.DeleteUser(id);
        }

        public async Task<Customer> RegisterUserCustomer(string login, string password, string[] customerData)
        {
            // roleId читателя: 1
            User user = await RegisterUser(login, password, 1);
            return await _customersRepository.AddCustomer(new Customer
            {
                FirstName = customerData[0],
                Patronymic = customerData[1],
                LastName = customerData[2],
                Phone = customerData[3],
                AltPhone = customerData[4],
                Address = customerData[5],
                UserId = user.Id
            });
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _customersRepository.GetAllCustomers();
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _customersRepository.GetCustomer(id) ?? throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
        }

        internal async Task<Customer> CreateCustomer(Customer customer)
        {
            return await _customersRepository.AddCustomer(customer);
        }

        internal async Task<Customer> EditCustomer(int id, Customer customer)
        {
            return await _customersRepository.EditCustomer(id, customer);
        }

        internal async Task<Customer> DeleteCustomer(int id)
        {
            return await _customersRepository.DeleteCustomer(id) ?? throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
        }
    }
}
