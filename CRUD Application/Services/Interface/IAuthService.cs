using CRUD_Application.Dtos;

namespace CRUD_Application.Services.Interface
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task RegisterAsync(RegisterDto dto);
        Task<LoginResponseDto> RefreshTokenAsync(
            string refreshToken);

        Task LogoutAsync(string refreshToken);
    }
}
