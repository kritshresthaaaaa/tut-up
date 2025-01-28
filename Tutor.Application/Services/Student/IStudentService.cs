using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutor.Application.Common.Interfaces;
using Tutor.Application.Common.Model.Response;
using Tutor.Application.Dtos.Student;

namespace Tutor.Application.Services.Student
{
    public interface IStudentService
    {
        Task<Response> CreateStudentAsync(CreateStudentRequest request);
        Task<Response> UpdateStudentAsync(Guid id, UpdateStudentRequest request);
        Task<GenericResponse<IEnumerable<GetAllStudentsResponse>>> GetAllStudentsAsync();
    }
}
