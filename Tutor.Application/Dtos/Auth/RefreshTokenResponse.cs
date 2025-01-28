using Tutor.Domain.Entities;

namespace Tutor.Application.Dtos.Auth
{
    public class RefreshTokenResponse
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string CreatedByIp { get; set; }
        public string ReplacedByToken { get; set; }
    }
}
