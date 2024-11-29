using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CBSEWebPortal.Models;
using CBSEWebPortal.Services;

namespace CBSEWebPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Principal")]
    public class PrincipalController : ControllerBase
    {
        private readonly IPrincipalService _principalService;

        public PrincipalController(IPrincipalService principalService)
        {
            _principalService = principalService;
        }

        [HttpGet("{principalId}/details")]
        public async Task<IActionResult> GetPrincipalDetails(int principalId)
        {
            var result = await _principalService.GetPrincipalDetailsAsync(principalId);

            if (result == null)
                return NotFound("Principal not found.");

            return Ok(result);
        }
        [HttpGet("student/{studentId}/marks")]
        public async Task<IActionResult> GetStudentMarks(int studentId)
        {
            var marks = await _principalService.GetStudentMarksAsync(studentId);
            if (marks == null)
                return NotFound("Marks for the specified student not found.");

            return Ok(marks);
        }

        [Authorize]
        [HttpPost("{principalId}/enroll-student")]
        public async Task<IActionResult> EnrollStudent(int principalId, [FromBody] StudentRequest request)
        {
            // Console.WriteLine(request);
            if (request == null || string.IsNullOrWhiteSpace(request.StudentName))
            {
                return BadRequest("Student name is required");
            }
            var result = await _principalService.EnrollStudentAsync(principalId, request.StudentName);

            if (result == null)
            {
                return NotFound("Principal not found");
            }
            return Ok(result);
        }

        [HttpGet("{principalId}/students")]
        public async Task<IActionResult> GetStudents(int principalId)
        {
            var students = await _principalService.GetMyStudentsAsync(principalId);

            if (students == null)
            {
                return NotFound("Principal not found");
            }

            return Ok(students);
        }
    }

}
