using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Week8.Dto;
using Week8.Services.AuthService;

namespace Week8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<string>>> Register(RegisterDto user){
            return Ok(await _authService.Register(user));
        }
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(LoginDto user){
            var response = await _authService.Login(user);
            if(response.Success == false) {
                NotFound(response);
            }
            return Ok(response);
        }

    }
}