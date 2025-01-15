using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tutor.Application.Common.Interfaces;
using Tutor.Application.Common.Model.Error;
using Tutor.Application.Common.Model.Response;
using Tutor.Application.Dtos.Student;
using Tutor.Domain.Entities;

namespace Tutor.Application.Services.Student
{
    public class StudentService : IStudentService
    {
        private readonly UserManager<User> _userManager;
        private readonly IGenericRepository<Tutor.Domain.Entities.Student> _studentRepository;
        private readonly IGenericRepository<Education> _educationRepository;
        public StudentService(UserManager<User> userManager, IGenericRepository<Tutor.Domain.Entities.Student> studentRepository, IGenericRepository<Education> educationRepository)
        {
            _userManager = userManager;
            _studentRepository = studentRepository;
            _educationRepository = educationRepository;
        }
        public async Task<Response> CreateStudentAsync(CreateStudentRequest request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                DateOfBirth = request.DOB,
                Email = request.Email,
                Gender = request.Gender,
                ProfilePictureUrl = request.ProfilePicture,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
            };
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return new ErrorModel(System.Net.HttpStatusCode.BadRequest, "Error while creating User!");
            }
            var student = new Tutor.Domain.Entities.Student
            {
                UserId = user.Id,
                GuardianName = request.GuardianName,
                GuardianContact = request.GuardianContact
            };
            if (request.Educations != null && request.Educations.Any())
            {
                student.Educations = request.Educations.Select(e => new Education
                {
                    UserId = user.Id,
                    Degree = e.Degree,
                    Institution = e.Institution,
                    Stream = e.Stream, // need to chaange in model nullable
                    Grade = e.Grade,  // need to chaange in model nullable
                    StartDate = e.StartDate,
                    EndDate = e.EndDate
                }).ToList();
            }

            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();
            await _educationRepository.SaveChangesAsync();
            return new Response();
        }
        public async Task<GenericResponse<IEnumerable<GetAllStudentsResponse>>> GetAllStudentsAsync()
        {
            var students = await (from s in _studentRepository.TableNoTracking
                                  join u in _userManager.Users on s.UserId equals u.Id
                                  join e in _educationRepository.TableNoTracking on s.UserId equals e.UserId into educations // group join educations contain all educations of a student
                                  /*
                                    "Student": { "UserId": 1, "StudentId": 1001, "GuardianName": "Alice's Mom", "GuardianContact": "123-456" },
                                     "User": { "Id": 1, "FirstName": "Alice", "LastName": "Smith", "Email": "alice@email.com", "Phone": "111-111" },
                                     "Educations": [
                                        { "Degree": "BSc", "Institution": "ABC College", "Grade": "A" },
                                        { "Degree": "MSc", "Institution": "XYZ Univ.", "Grade": "B" }
                                      ]
                                  */
                                  select new
                                  {
                                      Student = s,
                                      User = u,
                                      Educations = educations
                                  }).ToListAsync();

            var response = students.Select(s => new GetAllStudentsResponse
            {
                UserId = s.Student.UserId,
                StudentId = s.Student.Id,
                FullName = s.User.FirstName + " " + s.User.LastName,
                Email = s.User.Email,
                Phone = s.User.PhoneNumber,
                GuardianContact = s.Student.GuardianContact,
                GuardianName = s.Student.GuardianName,
                Address = s.User.Address,
                StudentsEducation = s.Educations.Select(e => new GetAllStudentsEducationResponse
                {
                    Degree = e.Degree,
                    Institution = e.Institution,
                    Stream = e.Stream,
                    Grade = e.Grade,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate
                }).ToList()
            }).ToList();

            return response;


        }
    }
}
