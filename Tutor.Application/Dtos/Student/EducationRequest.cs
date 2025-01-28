using Tutor.Domain.Enums;

namespace Tutor.Application.Dtos.Student
{
    public class EducationRequest
    {
        public EducationDegree Degree { get; set; }
        public string Institution { get; set; }
        public string? Stream { get; set; }
        public string? Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
