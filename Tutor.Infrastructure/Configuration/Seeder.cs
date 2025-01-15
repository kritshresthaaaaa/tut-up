using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Tutor.Domain.Entities;
using Tutor.Domain.Enums;

namespace Tutor.Infrastructure.Configuration
{
    public class Seeder:ISeeder
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public Seeder(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task SeedDataAsync()
        {
            var roles = Enum.GetValues<RoleEnum>();
            foreach (var roleType in roles)
            {
                if (Enum.IsDefined(typeof(RoleEnum), roleType) && !await _roleManager.RoleExistsAsync(roleType.ToString()))
                {
                    await _roleManager.CreateAsync(new Role
                    {
                        Name = roleType.ToString(),
                        RoleType = roleType,
                        CreationTime = DateTime.UtcNow,
                    });
                }
            }



            var adminEmail = "Admin@example.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreationTime = DateTime.UtcNow,
                    Address = "Nepal"
                };
                var result = await _userManager.CreateAsync(newAdmin, "Admin@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }

    }
}
