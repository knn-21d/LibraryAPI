using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Services;
using System.Web.Http;
using LibraryAPI.Data;
using LibraryAPI.Data.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LibraryAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class AnonymousController : ControllerBase
    {
        private readonly UserManagementService _userManagementService;
        private readonly BooksService _booksService;

        public AnonymousController(UserManagementService userManagementService, BooksService booksService)
        {
            _userManagementService = userManagementService;
            _booksService = booksService;
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

        [Microsoft.AspNetCore.Mvc.HttpPost("login")]
        public async Task<ActionResult<string>> Login([Microsoft.AspNetCore.Mvc.FromBody] LoginDTO loginRequest)
        {
            return await _userManagementService.LoginUser(loginRequest.Login, loginRequest.Password);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("searchBooks")]
        public async Task<ActionResult<List<Book>>> SearchBooks([Microsoft.AspNetCore.Mvc.FromQuery] BooksSearchDto booksSearchRequest)
        {
            return await _booksService.SearchBooks(booksSearchRequest);
        }
    }
}
