using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace LibraryAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("/api/[controller]")]
    [ApiController]
    public class LibrarianController : Controller
    {
        private readonly User _user;
        private readonly OrderService _orderService;
        private readonly StorageManagementService _storageManagementService;
        private readonly UserManagementService _userManagementService;

        public LibrarianController(User user, OrderService orderService, StorageManagementService storageManagementService, UserManagementService registerService) : base()
        {
            _user = user;
            _orderService = orderService;
            _storageManagementService = storageManagementService;
            _userManagementService = registerService;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("order/{orderId}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            try
            {
                return Ok(await _orderService.GetOrder(id, _user));
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("orders/all")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            try
            {
                return Ok(await _orderService.GetAllOrders(_user));
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("orders/customer={customerId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrdersByCustomer(int customerId)
        {
            try
            {
                return Ok(await _orderService.GetAllOrdersByCustomer(customerId, _user));
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode);
            }
        }

        // POST api/<CustomersController>
        [Microsoft.AspNetCore.Mvc.HttpPost("new-order/{customerId}/{isbn}")]
        public async Task<ActionResult<Order>> CreateOrder(int customerId, string isbn)
        {
            try
            {
                return Ok(await _orderService.CreateOrder(customerId, isbn));
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPatch("borrow/{orderId}")]
        public async Task<ActionResult<Order>> ReleaseCopy(int orderId)
        {
            try
            {
                return Ok(await _orderService.SetBorrowedNow(orderId));
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPatch("return/{orderId}")]
        public async Task<ActionResult<Order>> ReturnCopy(int orderId)
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
    }
}
