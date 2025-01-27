using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Tutor.Application.Services.CurrentUser
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UserId
        {
            get
            {
                var userIdCliam = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                var validGuid = string.IsNullOrEmpty(userIdCliam.Value) ? Guid.Empty : Guid.Parse(userIdCliam.Value);
                return validGuid;
            }
        }
    }
}
