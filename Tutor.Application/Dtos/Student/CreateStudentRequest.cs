using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutor.Domain.Entities;
using Tutor.Domain.Enums;

namespace Tutor.Application.Dtos.Student
{
    public class EducationRequest
    {
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string ?Stream { get; set; }
        public string ?Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
    public class CreateStudentRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string GuardianName { get; set; }
        public string GuardianContact { get; set; }
        public string ?ProfilePicture { get; set; }
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; }
        public IList<EducationRequest> Educations { get; set; }
    }
}
