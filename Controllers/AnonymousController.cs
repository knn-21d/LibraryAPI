using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Services;
using System.Web.Http;
using LibraryAPI.Data;
using LibraryAPI.Data.DTOs;

namespace LibraryAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class AnonymousController : ControllerBase
    {
        private readonly UserManagementService _userManagementService;

        public AnonymousController(UserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        // POST api/<AnonymousController>
        [Microsoft.AspNetCore.Mvc.HttpPost("register")]
        public async Task<ActionResult<Customer>> SignUpAsCustomer([Microsoft.AspNetCore.Mvc.FromBody] RegisterDTO userdata)
        {
            try
            {
                return await _userManagementService.RegisterUserCustomer(userdata.Login, userdata.Password, userdata.CustomerData);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode);
            }
        }
    }
}
