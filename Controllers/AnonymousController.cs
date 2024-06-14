using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Services;
using LibraryAPI.Models;
using System.Web.Http;

namespace LibraryAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class AnonymousController : ControllerBase
    {
        private readonly UserManagementService _userManagementService;

        public AnonymousController(UserManagementService registerService)
        {
            _userManagementService = registerService;
        }

        // POST api/<AnonymousController>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<ActionResult<Customer>> SignUpAsCustomer(string login, string password, string[] customerData)
        {
            try
            {
                return await _userManagementService.RegisterUserCustomer(login, password, customerData);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode);
            }
        }
    }
}
