using Tutor.Domain.Interfaces;

namespace Tutor.Domain.Entities.Shared
{
    public abstract class BaseEntity : Entity, IAuditableEntity, ISoftDeleteEntity
    {
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual Guid? DeletedBy { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public Guid? LastModifiedBy { get; set; }
    }
}
