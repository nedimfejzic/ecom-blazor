using Blazor.Server.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
        {
            var response = await _authService.Register(new User { Email = request.Email }, request.Password);

            if (!response.Sucess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDTO request)
        {
            var response = await _authService.Login(request.Email, request.Password);
            
            if (!response.Sucess)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }


    }
}
