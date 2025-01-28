using Tutor.Domain.Enums;

namespace Tutor.Application.Dtos.Student
{
    public class UpdateStudentRequest
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string GuardianName { get; set; }
        public string GuardianContact { get; set; }
        public string? ProfilePicture { get; set; }
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; }
        public IList<EducationRequest> Educations { get; set; }
    }
}
