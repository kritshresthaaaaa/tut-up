using Tutor.Domain.Entities.Shared;

namespace Tutor.Domain.Entities
{
    public class Student : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string GuardianName { get; set; }
        public string GuardianContact { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
    }
}
