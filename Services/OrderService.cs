using LibraryAPI.Data;
using LibraryAPI.Data.Repositories;
using System.Web.Http;

namespace LibraryAPI.Services
{
    public class OrderService
    {
        private readonly OrdersRepository _ordersRepository;
        private readonly CustomersRepository _customersRepository;
        private readonly BooksRepository _booksRepository;
        private readonly CopiesRepository _copiesRepository;

        public OrderService(OrdersRepository ordersRepository, CustomersRepository customersRepository, BooksRepository booksRepository, CopiesRepository copiesRepository)
        {
            _ordersRepository = ordersRepository;
            _customersRepository = customersRepository;
            _booksRepository = booksRepository;
            _copiesRepository = copiesRepository;
        }

        public async Task<Order> GetOrder(int id, User requestSender)
        {
            Order? order = await _ordersRepository.GetOrder(id);

            if (order is null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            Customer? customer = await _customersRepository.GetCustomerByUserId(requestSender.Id);

            if (Enumerable.Range(2, 3).Contains(requestSender.RoleId) || order.CustomerId == customer?.Id)
            {
                return order;
            }
            else
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrders(User requestSender)
        {
            if (Enumerable.Range(2, 3).Contains(requestSender.RoleId))
            {
                return await _ordersRepository.GetAllOrders();
            }
            else
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByCustomer(User requestSender, int? customerId)
        {
            Customer? customer = customerId is not null
                ? await _customersRepository.GetCustomer((int)customerId)
                : await _customersRepository.GetCustomerByUserId(requestSender.Id);
            if (customer is null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }


            if (Enumerable.Range(2, 3).Contains(requestSender.RoleId) || customerId is null)
            {
                return await _ordersRepository.GetAllOrdersByCustomer(customer.Id);
            }
            else
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }
        }

        public async Task<Order> CreateOrder(int customerId, string isbn)
        {
            Customer? customer = await _customersRepository.GetCustomer(customerId);
            Book? book = await _booksRepository.GetBook(isbn);

            if (customer is null || book is null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }

            Copy copy = await _copiesRepository.GetAnyCopy(book) ?? throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            Order order = new Order { CustomerId = customerId, CopyId = copy.Id, Created = DateTime.Now };
            await _ordersRepository.AddOrder(order);

            return order;
        }

        public async Task<Order> SetBorrowedNow(int orderId)
        {
            Order order = await _ordersRepository.GetOrder(orderId) ?? throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            if (order.Borrowed is null && order.Returned is null)
            {
                await _ordersRepository.SetBorrowedNow(order);
                return order;
            }
            else
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }
        }

        public async Task<Order> SetReturnedNow(int orderId)
        {
            Order order = await _ordersRepository.GetOrder(orderId) ?? throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            if (order.Returned is null && order.Borrowed is not null)
            {
                await _ordersRepository.SetReturnedNow(order);
                return order;
            }
            else
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
