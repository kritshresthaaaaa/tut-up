using Tutor.Application.Common.Model.Response;
using Tutor.Application.Dtos.Auth;

namespace Tutor.Application.Services.Authentication
{
    public interface IAuthService
    {
        Task<GenericResponse<AuthResponse>> AuthenticateAsync(AuthenticateRequest request);

    }
}
