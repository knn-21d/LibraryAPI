using LibraryAPI.Data;
using LibraryAPI.Data.DTOs;
using LibraryAPI.Data.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web.Helpers;
using System.Web.Http;

namespace LibraryAPI.Services
{
    public class UserManagementService
    {
        private readonly UsersRepository _usersRepository;
        private readonly CustomersRepository _customersRepository;
        private readonly IConfiguration _config;


        public UserManagementService(UsersRepository usersRepository, CustomersRepository customersRepository, IConfiguration config)
        {
            _customersRepository = customersRepository;
            _usersRepository = usersRepository;
            _config = config;
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

        public async Task<string> LoginUser(string login, string password)
        {
            User? user = await _usersRepository.GetuserByLogin(login);
            if (user is null)
            {
                throw new UnauthorizedAccessException();
            }
            var isCorrectPassword = Crypto.VerifyHashedPassword(user.PasswordHash, password);
            if (!isCorrectPassword)
            {
                throw new UnauthorizedAccessException();
            }
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var customer = await this._customersRepository.GetCustomerByUserId(user.Id);

            var claims = new List<Claim>
            {
                new Claim("user", JsonSerializer.Serialize(new UserJwt{Id = user.Id, Login = user.Login, PasswordHash = user.PasswordHash, Role = user.Role, RoleId = user.RoleId}, new JsonSerializerOptions{ReferenceHandler = ReferenceHandler.Preserve})),
                new Claim("customer", JsonSerializer.Serialize(new CustomerJwt{
                    Address = customer?.Address, 
                    AltPhone = customer?.AltPhone, 
                    FirstName = customer?.FirstName, 
                    Id = customer!.Id, 
                    LastName = customer?.LastName, 
                    Patronymic = customer?.Patronymic, 
                    Phone = customer?.Phone, 
                    UserId = customer!.UserId
                }, new JsonSerializerOptions{ReferenceHandler = ReferenceHandler.Preserve}))
            };

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  claims,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                return token;
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
