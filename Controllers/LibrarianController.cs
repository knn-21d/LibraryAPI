using LibraryAPI.Data;
using LibraryAPI.Data.DTOs;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Web.Http;

namespace LibraryAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("/api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize()]
    public class LibrarianController : Controller
    {
        private readonly User? _user = null;
        private readonly OrderService _orderService;
        private readonly StorageManagementService _storageManagementService;

        public LibrarianController(OrderService orderService, StorageManagementService storageManagementService) : base()
        {
            _orderService = orderService;
            _storageManagementService = storageManagementService;
        }

        [Microsoft.AspNetCore.Authorization.Authorize()]
        [Microsoft.AspNetCore.Mvc.HttpGet("order/{orderId}")]
        public async Task<ActionResult<Data.Order>> GetOrder(int orderId)
        {
            var user = JsonSerializer.Deserialize<User>(User.Claims.FirstOrDefault(claim => claim?.Type == "user")?.Value!)!;
            if (!Enumerable.Range(2, 3).Contains(user.RoleId))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }
            return Ok(await _orderService.GetOrder(orderId, user));
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("orders/all")]
        [Microsoft.AspNetCore.Authorization.Authorize()]
        public async Task<ActionResult<IEnumerable<Data.Order>>> GetAllOrders()
        {
            var user = JsonSerializer.Deserialize<User>(User.Claims.FirstOrDefault(claim => claim?.Type == "user")?.Value!)!;
            if (!Enumerable.Range(2, 3).Contains(user.RoleId))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }
            return Ok(await _orderService.GetAllOrders(user));
        }

        [Microsoft.AspNetCore.Authorization.Authorize()]
        [Microsoft.AspNetCore.Mvc.HttpGet("orders/customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Data.Order>>> GetAllOrdersByCustomer(int customerId)
        {

            var user = JsonSerializer.Deserialize<User>(User.Claims.FirstOrDefault(claim => claim?.Type == "user")?.Value!)!;
            if (!Enumerable.Range(2, 3).Contains(user.RoleId))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }
            return Ok(await _orderService.GetAllOrdersByCustomer(user, customerId));
        }

        // POST api/<CustomersController>
        [Microsoft.AspNetCore.Mvc.HttpPost("new-order/{customerId}/{isbn}")]
        [Microsoft.AspNetCore.Authorization.Authorize()]
        public async Task<ActionResult<Data.Order>> CreateOrder(int customerId, string isbn)
        {
            var user = JsonSerializer.Deserialize<User>(User.Claims.FirstOrDefault(claim => claim?.Type == "user")?.Value!)!;
            if (!Enumerable.Range(2, 3).Contains(user.RoleId))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }
            return Ok(await _orderService.CreateOrder(customerId, isbn));
        }

        [Microsoft.AspNetCore.Authorization.Authorize()]
        [Microsoft.AspNetCore.Mvc.HttpPatch("borrow/{orderId}")]
        public async Task<ActionResult<Data.Order>> ReleaseCopy(int orderId)
        {
            var user = JsonSerializer.Deserialize<User>(User.Claims.FirstOrDefault(claim => claim?.Type == "user")?.Value!)!;
            if (!Enumerable.Range(2, 3).Contains(user.RoleId))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }
            try
            {
                return Ok(await _orderService.SetBorrowedNow(orderId));
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode);
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize()]
        [Microsoft.AspNetCore.Mvc.HttpPatch("return/{orderId}")]
        public async Task<ActionResult<Data.Order>> ReturnCopy(int orderId)
        {
            try
            {
                return Ok(await _orderService.SetReturnedNow(orderId));
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode);
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize()]
        [Microsoft.AspNetCore.Mvc.HttpPost("addBook")]
        public async Task<ActionResult<Book>> AddBook([Microsoft.AspNetCore.Mvc.FromBody] BookDto book)
        {
            var user = JsonSerializer.Deserialize<User>(User.Claims.FirstOrDefault(claim => claim?.Type == "user")?.Value!)!;
            if (!Enumerable.Range(2, 3).Contains(user.RoleId))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }

            var createdBook = await this._storageManagementService.AddBoock(book);
            return createdBook is not null ? createdBook : StatusCode(400);
        }

        [Microsoft.AspNetCore.Authorization.Authorize()]
        [Microsoft.AspNetCore.Mvc.HttpPost("addCopies")]
        public async Task<ActionResult<List<Copy>>> AddCopies(AddCopiesDto addCopiesDto)
        {
            var user = JsonSerializer.Deserialize<User>(User.Claims.FirstOrDefault(claim => claim?.Type == "user")?.Value!)!;
            if (!Enumerable.Range(2, 3).Contains(user.RoleId))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }
            return (await this._storageManagementService.AddNCopies(addCopiesDto.isbn, addCopiesDto.count)).ToList();
        }
    }
}
