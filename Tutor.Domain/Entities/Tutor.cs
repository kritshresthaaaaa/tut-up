using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutor.Domain.Entities.Shared;

namespace Tutor.Domain.Entities
{
    public class Tutor : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int YOE { get; set; }// Years of Experience
        public string Qualification { get; set; }
        public string Bio { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
    }
}
