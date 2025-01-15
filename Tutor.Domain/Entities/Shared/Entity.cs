using System.ComponentModel.DataAnnotations;

namespace Tutor.Domain.Entities.Shared
{
    public class Entity
    {
        [Key]
        public virtual Guid Id { get; set; }
    }
}
