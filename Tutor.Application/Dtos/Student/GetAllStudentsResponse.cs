using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutor.Domain.Enums;

namespace Tutor.Application.Dtos.Student
{
    public class GetAllStudentsEducationResponse
    {
        public EducationDegree Degree { get; set; }
        public string Institution { get; set; }
        public string? Stream { get; set; }
        public string? Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public record GetAllStudentsResponse
    {
        public Guid UserId { get; set; }
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GuardianName { get; set; }
        public string GuardianContact { get; set; }
        public string Address { get; set; }
        public IList<GetAllStudentsEducationResponse> StudentsEducation { get; set; }
    }
}
