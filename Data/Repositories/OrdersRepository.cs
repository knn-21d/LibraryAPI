using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data.Repositories
{
    public class OrdersRepository
    {
        private readonly LibraryDbContext _context;

        public OrdersRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByCustomer(int customerId)
        {
            return await _context.Orders.Where(x => x.CustomerId == customerId).ToListAsync();
        }

        public async Task<Order?> GetOrder(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Order> AddOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> SetBorrowedNow(Order order)
        {
            order.Borrowed = DateTime.Now;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> SetReturnedNow(Order order)
        {
            order.Returned = DateTime.Now;
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
