namespace Tutor.Application.Dtos.Auth
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime TokenExpiresAt { get; set; }
    }
}
