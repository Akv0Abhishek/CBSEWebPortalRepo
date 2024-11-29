using CBSEWebPortal.Models;

namespace CBSEWebPortal.Services
{
    public interface ICBSEAdminService
    {
        Task<CBSEAdmin> GetAdminByEmailAsync(string email);
        Task<bool> ValidateAdminCredentialsAsync(string email, string password);
        Task<bool> AddAdminAsync(CBSEAdmin admin);
        Task<List<StudentRequest>> GetPendingStudentRequestsAsync();
        Task<StudentRequest> EnrollStudentAsync(StudentRequest request);
        Task<IEnumerable<School>> GetAllSchoolsAsync();
        Task<SubjectMarksDto> GetStudentMarksAsync(int studentId);
        Task<List<Student>> GetAllStudentsAsync();
        Task<SubjectMarks> UpdateSubjectMarksAsync(int studentId, SubjectMarksDto marksDto);
    }
}
