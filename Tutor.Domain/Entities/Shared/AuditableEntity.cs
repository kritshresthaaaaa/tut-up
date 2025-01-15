using Tutor.Domain.Interfaces;

namespace Tutor.Domain.Entities.Shared
{
    public class AuditableEntity : IAuditableEntity
    {
        public DateTime CreationTime { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public Guid? LastModifiedBy { get; set; }
    }
}
