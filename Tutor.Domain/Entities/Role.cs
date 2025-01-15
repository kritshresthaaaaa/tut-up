using Microsoft.AspNetCore.Identity;
using Tutor.Domain.Enums;
using Tutor.Domain.Interfaces;

namespace Tutor.Domain.Entities
{
    public class Role : IdentityRole<Guid>, ISoftDeleteEntity, IAuditableEntity
    {
        public RoleEnum RoleType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
