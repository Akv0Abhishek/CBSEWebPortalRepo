using CBSEWebPortal.Models;

namespace CBSEWebPortal.Services
{
    public interface IPrincipalService
    {
        Task<bool> AddPrincipalAsync(RegisterPrincipalRequest principal);
        Task<Principal> GetPrincipalByEmailAsync(string email);
        Task<bool> ValidatePrincipalCredentialsAsync(string email, string password);
        Task<PrincipalDetailsDto> GetPrincipalDetailsAsync(int principalId);
        Task<object> EnrollStudentAsync(int principalId, string studentName);
        Task<List<StudentDto>> GetMyStudentsAsync(int principalId);
        Task<SubjectMarksDto> GetStudentMarksAsync(int studentId);
    }
}
