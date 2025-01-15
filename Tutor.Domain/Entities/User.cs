using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Tutor.Domain.Enums;
using Tutor.Domain.Interfaces;

namespace Tutor.Domain.Entities
{
    public class User : IdentityUser<Guid>, ISoftDeleteEntity, IAuditableEntity, IEntity
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }

}
