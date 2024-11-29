using CBSEWebPortal.Models;
using CBSEWebPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace CBSEWebPortal.Services
{
    public class PrincipalService : IPrincipalService
    {
        private readonly AppDbContext _context;

        public PrincipalService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddPrincipalAsync(RegisterPrincipalRequest principal)
        {
            try
            {
                Console.WriteLine($"Email: {principal.Email}, Name: {principal.Name}, Password: {principal.Password}, SchoolName:{principal.SchoolName}");

                var existingPrincipal = await _context.Principal
                    .FirstOrDefaultAsync(p => p.Email == principal.Email);
                if (existingPrincipal != null)
                {
                    return false;

                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(principal.Password);

                var newPrincipal = new Principal
                {
                    Name = principal.Name,
                    Email = principal.Email,
                    Password = hashedPassword
                };

                if (!string.IsNullOrEmpty(principal.SchoolName))
                {
                    var existingSchool = await _context.School
                        .FirstOrDefaultAsync(s => s.Name == principal.SchoolName);
                    if (existingSchool == null)
                    {
                        var newSchool = new School
                        {
                            Name = principal.SchoolName
                        };

                        _context.School.Add(newSchool);
                        await _context.SaveChangesAsync();

                        newPrincipal.SchoolID = newSchool.ID;
                    }
                    else
                    {
                        newPrincipal.SchoolID = existingSchool.ID;
                    }
                }

                _context.Principal.Add(newPrincipal);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding principal: {ex.Message}");
                return false;
            }


        }

        public async Task<Principal> GetPrincipalByEmailAsync(string email)
        {
            return await _context.Principal.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<bool> ValidatePrincipalCredentialsAsync(string email, string password)
        {
            var principal = await GetPrincipalByEmailAsync(email);
            return principal != null && BCrypt.Net.BCrypt.Verify(password, principal.Password);
        }

        public async Task<object> EnrollStudentAsync(int principalId, string studentName)
        {
            var principal = await _context.Principal.FirstOrDefaultAsync(p => p.ID == principalId);

            if (principal == null) return null;

            var istTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            var studentRequest = new StudentRequest
            {
                PrincipalId = principalId,
                StudentName = studentName,
                Status = "Pending",
                CreatedAt = istTime
            };
            Console.WriteLine(studentRequest);
            await _context.StudentRequests.AddAsync(studentRequest);
            await _context.SaveChangesAsync();
            return studentRequest;
        }
        public async Task<PrincipalDetailsDto> GetPrincipalDetailsAsync(int principalId)
        {
            var principal = await _context.Principal
                .Include(p => p.School)
                .FirstOrDefaultAsync(p => p.ID == principalId);

            if (principal == null)
                return null;

            return new PrincipalDetailsDto
            {
                PrincipalName = principal.Name,
                SchoolName = principal.School.Name
            };
        }


        public async Task<List<StudentDto>> GetMyStudentsAsync(int principalId)
        {
            var principal = await _context.Principal
                .Include(p => p.School)
                .ThenInclude(s => s.Students)
                .FirstOrDefaultAsync(p => p.ID == principalId);

            if (principal == null) return null;

            var studentDtos = principal.School.Students
            .Select(s => new StudentDto
            {
                ID = s.ID,
                Name = s.Name,
                Roll_No = s.Roll_No
            })
            .ToList();
            return studentDtos;
        }

        public async Task<SubjectMarksDto> GetStudentMarksAsync(int studentId)
        {
            var student = await _context.Student
                .Include(s => s.SubjectMarks)
                .FirstOrDefaultAsync(s => s.ID == studentId);

            if (student == null || student.SubjectMarks == null)
                return null;

            return new SubjectMarksDto
            {
                Sub1 = student.SubjectMarks.Sub1,
                Sub2 = student.SubjectMarks.Sub2,
                Sub3 = student.SubjectMarks.Sub3,
                Sub4 = student.SubjectMarks.Sub4,
                Sub5 = student.SubjectMarks.Sub5
            };
        }

    }
}
