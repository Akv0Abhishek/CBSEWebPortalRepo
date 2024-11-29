using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CBSEWebPortal.Models;
using CBSEWebPortal.Services;
namespace CBSEWebPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]

    public class CBSEAdminController : ControllerBase
    {
        private readonly ICBSEAdminService _cbseAdminService;

        public CBSEAdminController(ICBSEAdminService cbseAdminService)
        {
            _cbseAdminService = cbseAdminService;
        }


        [Authorize]
        [HttpGet("schools")]
        public async Task<IActionResult> GetAllSchools()
        {
            var schools = await _cbseAdminService.GetAllSchoolsAsync();
            return Ok(schools);
        }

        [Authorize]
        [HttpGet("student/{studentId}/marks")]
        public async Task<IActionResult> GetStudentMarks(int studentId)
        {
            var marks = await _cbseAdminService.GetStudentMarksAsync(studentId);
            if (marks == null)
                return NotFound("Marks for the specified student not found.");
            return Ok(marks);
        }

        [Authorize]
        [HttpPut("{studentId}/marks")]
        public async Task<IActionResult> AddOrUpdateStudentMarks(int studentId, [FromBody] SubjectMarksDto marksDto)
        {
            if (marksDto == null)
                return BadRequest(new { message = "Marks data is required." });

            try
            {
                var updatedMarks = await _cbseAdminService.UpdateSubjectMarksAsync(studentId, marksDto);

                if (updatedMarks == null)
                    return NotFound(new { message = "Student not found." });

                return Ok(new
                {
                    message = "Subject marks updated successfully.",
                    data = updatedMarks
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        // Get all pending requests

        [Authorize]
        [HttpGet("student-requests")]
        public async Task<ActionResult<List<StudentRequest>>> GetPendingStudentRequests()
        {

            var requests = await _cbseAdminService.GetPendingStudentRequestsAsync();
            if (requests == null || requests.Count == 0)
            {
                return NotFound("No pending student requests.");
            }
            return Ok(requests);
        }

        // Enroll student based on request
        [Authorize]
        [HttpPost("enroll-student")]
        public async Task<ActionResult<StudentRequest>> EnrollStudent([FromBody] StudentRequest request)
        {
            var updatedRequest = await _cbseAdminService.EnrollStudentAsync(request);
            if (updatedRequest == null)
            {
                return BadRequest("Failed to enroll student.");
            }
            return Ok(updatedRequest);
        }

        [Authorize]
        [HttpGet("students")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _cbseAdminService.GetAllStudentsAsync();
            return Ok(students);
        }
    }

}
