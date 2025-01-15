using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Data
{
    public class TutorDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public TutorDbContext(DbContextOptions<TutorDbContext> options) : base(options)
        {
        }

        public DbSet<Education> Education { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Tutor.Domain.Entities.Tutor> Tutor { get; set; }
    }
}
