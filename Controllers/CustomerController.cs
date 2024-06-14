using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Web.Http;

namespace LibraryAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly User _user;
        private readonly Customer _customer;
        private readonly OrderService _orderService;

        public CustomerController(User user, Customer customer, OrderService orderService)
        {
            _user = user;
            _customer = customer;
            _orderService = orderService;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
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

        [Microsoft.AspNetCore.Mvc.HttpGet("customers/getOrdersByCustomer")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrdersByCustomer()
        {
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
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(string isbn)
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
