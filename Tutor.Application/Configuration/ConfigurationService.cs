using Microsoft.Extensions.DependencyInjection;
using Tutor.Application.Services.CurrentUser;
using Tutor.Application.Services.Student;

namespace MajaDum.Application.Configuration
{
    public static class ConfigurationService
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }
    }
}
