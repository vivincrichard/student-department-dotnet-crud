using CRUD_Application.Configrations;
using CRUD_Application.Dtos;
using CRUD_Application.Handlers.Exceptions;
using CRUD_Application.Models;
using CRUD_Application.Repositories.Interface;
using CRUD_Application.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CRUD_Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly PasswordHasher<object> _passwordHasher;

        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtSettings = jwtSettings.Value;

            _passwordHasher = new PasswordHasher<object>();
        }

        // =====================================================
        // REGISTER
        // =====================================================

        public async Task RegisterAsync(RegisterDto dto)
        {
            var email = dto.Email.Trim().ToLower();

            var existingUser =
                await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
                throw new Exception("Email already exists.");

            var user = new User
            {
                UserName = dto.UserName.Trim(),
                Email = email,
                IsActive = true
            };

            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    dto.Password);

            await _userRepository.AddAsync(user);
        }


        // =====================================================
        // LOGIN
        // =====================================================

        public async Task<LoginResponseDto> LoginAsync(
            LoginDto dto)
        {
            var email = dto.Email.Trim().ToLower();

            var user =
                await _userRepository.GetByEmailAsync(email);

            if (user == null)
                throw new Exception(
                    "Invalid email or password.");

            if (!user.IsActive)
                throw new Exception(
                    "User account is inactive.");

            var passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    dto.Password);

            if (passwordResult ==
                PasswordVerificationResult.Failed)
            {
                throw new Exception(
                    "Invalid email or password.");
            }

            return await GenerateTokenResponseAsync(user);
        }

        // =====================================================
        // REFRESH TOKEN
        // =====================================================

        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var existingToken =
                await _refreshTokenRepository
                    .GetByTokenAsync(refreshToken);

            if (existingToken == null)
            {
                throw new UnauthorizedException(
                    "Invalid refresh token.");
            }

            if (existingToken.RevokedAt != null)
            {
                throw new UnauthorizedException(
                    "Refresh token has been revoked.");
            }

            if (existingToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedException(
                    "Refresh token has expired.");
            }

            if (existingToken.User == null)
            {
                throw new UnauthorizedException(
                    "User associated with refresh token was not found.");
            }

            if (!existingToken.User.IsActive)
            {
                throw new ForbiddenException(
                    "User account is inactive.");
            }

            // Rotate refresh token
            existingToken.RevokedAt = DateTime.UtcNow;

            await _refreshTokenRepository
                .UpdateAsync(existingToken);

            // Generate new access + refresh token
            return await GenerateTokenResponseAsync(
                existingToken.User);
        }


        // =====================================================
        // LOGOUT
        // =====================================================

        public async Task LogoutAsync(
            string refreshToken)
        {
            var existingToken =
                await _refreshTokenRepository
                    .GetByTokenAsync(refreshToken);

            if (existingToken == null)
                return;

            if (existingToken.RevokedAt != null)
                return;

            existingToken.RevokedAt = DateTime.UtcNow;

            await _refreshTokenRepository
                .UpdateAsync(existingToken);
        }


        // =====================================================
        // GENERATE ACCESS + REFRESH TOKEN
        // =====================================================

        private async Task<LoginResponseDto>
            GenerateTokenResponseAsync(User user)
        {
            // ---------------------------------------------
            // Access Token
            // ---------------------------------------------

            var accessTokenExpiresAt =
                DateTime.UtcNow.AddMinutes(
                    _jwtSettings.ExpirationMinutes);

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.UserName),

                new Claim(
                    ClaimTypes.Email,
                    user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _jwtSettings.Key));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: accessTokenExpiresAt,
                signingCredentials: credentials);

            var accessToken =
                new JwtSecurityTokenHandler()
                    .WriteToken(jwtToken);


            // ---------------------------------------------
            // Refresh Token
            // ---------------------------------------------

            var refreshTokenValue =
                GenerateRefreshToken();

            var refreshTokenExpiresAt =
                DateTime.UtcNow.AddDays(7);

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenValue,

                UserId = user.UserId,

                CreatedAt = DateTime.UtcNow,

                ExpiresAt = refreshTokenExpiresAt
            };

            await _refreshTokenRepository
                .AddAsync(refreshToken);


            // ---------------------------------------------
            // Response
            // ---------------------------------------------

            return new LoginResponseDto
            {
                AccessToken = accessToken,

                RefreshToken = refreshTokenValue,

                AccessTokenExpiresAt =
                    accessTokenExpiresAt,

                RefreshTokenExpiresAt =
                    refreshTokenExpiresAt
            };
        }


        // =====================================================
        // GENERATE SECURE RANDOM REFRESH TOKEN
        // =====================================================

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var randomNumberGenerator =
                RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(
                randomBytes);

            return Convert.ToBase64String(
                randomBytes);
        }
    }
}

