using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutor.Application.Common.Model.Response;
using Tutor.Application.Dtos.Auth;
using Tutor.Application.Services.Authentication;

namespace Tutor.API.Controllers
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
        [HttpPost("login")]
        public async Task<ActionResult<GenericResponse<AuthResponse>>> Login([FromBody] AuthenticateRequest request)
        {
            var response = await _authService.AuthenticateAsync(request);
            return Ok(response);
        }
    }
}
