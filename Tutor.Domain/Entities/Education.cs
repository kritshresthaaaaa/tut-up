using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutor.Domain.Entities.Shared;

namespace Tutor.Domain.Entities
{
    public class Education:BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string? Stream { get; set; }
        public string? Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
