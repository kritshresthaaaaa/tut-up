using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutor.Application.Common.Interfaces;
using Tutor.Application.Common.Model.Error;
using Tutor.Application.Common.Model.Response;
using Tutor.Application.Dtos.Student;
using Tutor.Application.Services.Student;

namespace Tutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpPost("create")]
        public async Task<ActionResult<Response>> CreateStudentAsync([FromBody] CreateStudentRequest request)
        {
            var response = await _studentService.CreateStudentAsync(request);
            if (!response.IsSuccess)
            {
                return response;
            }
            return Ok(response);
        }
        [HttpGet]
        public async Task<ActionResult<IGenericRepository<IEnumerable<GetAllStudentsResponse>>>> GetAllStudentsAsync()
        {
            var response = await _studentService.GetAllStudentsAsync();
            return Ok(response);
        }

    }
}
