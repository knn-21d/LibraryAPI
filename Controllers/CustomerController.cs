using LibraryAPI.Data;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace LibraryAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly User? _user = null;
        private readonly Customer? _customer = null;
        private readonly OrderService _orderService;

        public CustomerController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("order/{id}")]
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

        [Microsoft.AspNetCore.Mvc.HttpGet("orders")]
        [Authorize()]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrdersByCustomer()
        {
            var test = User;
            try
            {
                return Ok(await _orderService.GetAllOrdersByCustomer(_customer.Id, _user));
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode);
            }
        }

        // POST api/<CustomersController>
        [Microsoft.AspNetCore.Mvc.HttpPost("new-order")]
        public async Task<ActionResult<Order>> CreateOrder([Microsoft.AspNetCore.Mvc.FromBody] string isbn)
        {
            try
            {
                return Ok(await _orderService.CreateOrder(_customer.Id, isbn));
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode);
            }
        }
    }
}
