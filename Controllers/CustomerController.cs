using LibraryAPI.Data;
using LibraryAPI.Data.DTOs;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text.Json;
using System.Web.Http;
using System.Web.Http.Results;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace LibraryAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly OrderService _orderService;

        public CustomerController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("order/{id}")]
        [Authorize()]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var user = JsonSerializer.Deserialize<User>(User.Claims.FirstOrDefault(claim => claim?.Type == "user")!.Value!);
            return Ok(await _orderService.GetOrder(id, user));
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("orders")]
        [Authorize()]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrdersByCustomer()
        {
            var user = User;
            var userObj = JsonSerializer.Deserialize<User>(user.Claims.FirstOrDefault(claim => claim?.Type == "user")?.Value!)!;
            return Ok(await _orderService.GetAllOrdersByCustomer(userObj, null));
        }

        // POST api/<CustomersController>
        [Microsoft.AspNetCore.Mvc.HttpPost("new-order")]
        [Authorize()]
        public async Task<ActionResult<Order>> CreateOrder([Microsoft.AspNetCore.Mvc.FromBody] NewOrderDto isbn)
        {
            var customer = JsonSerializer.Deserialize<Customer>(User.Claims.FirstOrDefault(claim => claim?.Type == "customer")!.Value!);
            return Ok(await _orderService.CreateOrder(customer!.Id, isbn.isbn));
        }
    }
}
