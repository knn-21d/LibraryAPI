using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data.Repositories
{
    public class CustomersRepository
    {
        private readonly LibraryDbContext _context;

        public CustomersRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _context.Customers.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Customer?> GetCustomer(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Customer?> GetCustomerByUserId(int id)
        {
            return await _context.Customers.Where(x => x.UserId == id).FirstOrDefaultAsync();
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        internal async Task<Customer> EditCustomer(int id, Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        internal async Task<Customer?> DeleteCustomer(int id)
        {
            await _context.Customers.Where(x => x.Id == id).ExecuteDeleteAsync();
            return null;
        }
    }
}
