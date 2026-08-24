using CRUD_Application.Dtos;
using CRUD_Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Application.Controllers
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
        public async Task<IActionResult> Register(
            [FromBody] RegisterDto dto)
        {
            await _authService.RegisterAsync(dto);

            return Ok(new
            {
                message = "User registered successfully."
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            return Ok(result);
        }


        // =====================================================
        // REFRESH TOKEN
        // =====================================================

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenDto dto)
        {
            var result =
                await _authService
                    .RefreshTokenAsync(
                        dto.RefreshToken);

            return Ok(result);
        }


        // =====================================================
        // LOGOUT
        // =====================================================

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            [FromBody] RefreshTokenDto dto)
        {
            await _authService
                .LogoutAsync(
                    dto.RefreshToken);

            return Ok(new
            {
                message = "Logged out successfully."
            });
        }
    }
}
