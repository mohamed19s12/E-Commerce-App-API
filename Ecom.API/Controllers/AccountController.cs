using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IMapper mapper, IUnitOfWork work) : base(mapper, work)
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var result = await work.Auth.RegisterAsync(registerDTO);
            if (result != "done")
                return BadRequest( new ResponseAPI(400 , result));
            return Ok(new ResponseAPI(200, result));
        }

        [HttpPost("Login")]
        public async Task<IActionResult>  Login(LoginDTO loginDTO)
        {
            //all errors start with "please"
            var result = await work.Auth.LoginAsync(loginDTO);
            if (result.StartsWith("please"))
            {
                return BadRequest( new ResponseAPI(400 ,result));
            }

            Response.Cookies.Append("Token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTime.UtcNow.AddDays(1),
                IsEssential = true,
                SameSite =SameSiteMode.Strict
            });
            return Ok(new ResponseAPI(200, result));

        }
        [HttpPost("active-account")]
        public async Task<IActionResult> Active(ActiveEmailDTO activeEmailDTO)
        {
            var result = await work.Auth.ActiveAccountAsync(activeEmailDTO);

            //for each true or false return response api with status code 200
            return result ? Ok(new ResponseAPI(200)) :
                BadRequest(new ResponseAPI(200));
        }
        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> SendEmailForgetPassword([FromQuery] string email)
        {
            var result = await work.Auth.SendEmailForGorgetPasswordAsync(email);
            return result ? Ok(new ResponseAPI(200)) :
               BadRequest(new ResponseAPI(200));
        }
    }
}
