using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tutor.Application.Common.Interfaces;
using Tutor.Application.Common.Model.Error;
using Tutor.Application.Common.Model.Response;
using Tutor.Application.Dtos.Auth;
using Tutor.Domain.Entities;

namespace Tutor.Application.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IConfiguration configuration, IGenericRepository<RefreshToken> refreshTokenRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GenericResponse<AuthResponse>> AuthenticateAsync(AuthenticateRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new ErrorModel(System.Net.HttpStatusCode.BadRequest, "User not found!");
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (result)
            {
                var token = await GenerateJwtToken(user);
                if (string.IsNullOrEmpty(token))
                {
                    return new ErrorModel(System.Net.HttpStatusCode.BadRequest, "Error while generating token!");
                }
                var role = await _userManager.GetRolesAsync(user);
                var response = new AuthResponse
                {
                    AccessToken = token,
                    Email = user.Email!,
                    FullName = user.FirstName + " " + user.LastName,
                    Role = role.FirstOrDefault()!.ToString()
                };
                var refreshToken = await GenerateRefreshTokenAsync(user.Id, _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
                return response;
            }
            else
            {
                return new ErrorModel(System.Net.HttpStatusCode.BadRequest, "Invalid password!");
            }

        }
        private async Task<string> GenerateJwtToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var description = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpirationTimeInMinutes"]!))
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenCreator = tokenHandler.CreateJwtSecurityToken(description);
            var token = tokenHandler.WriteToken(tokenCreator);
            return token;
        }
        private async Task<RefreshTokenResponse> GenerateRefreshTokenAsync(Guid userId, string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Expires = DateTime.UtcNow.AddDays(double.Parse(_configuration["RefreshToken:ExpirationTimeInDays"]!)),
                UserId = userId,
                CreatedByIp = ipAddress
            };
            await _refreshTokenRepository.AddAsync(refreshToken);
            var response = new RefreshTokenResponse
            {
                Token = refreshToken.Token,
                Expires = refreshToken.Expires,
                UserId = refreshToken.UserId,
                CreatedByIp = refreshToken.CreatedByIp
            };
            return response;
        }
        public  string GeneratePasswordResetLink(string email, string token)
        {
            var encodedToken = Uri.EscapeDataString(token);

            var resetUrl = $"https://yourapp.com/reset-password?email={email}&token={encodedToken}";

            return resetUrl;
        }
    }
}
