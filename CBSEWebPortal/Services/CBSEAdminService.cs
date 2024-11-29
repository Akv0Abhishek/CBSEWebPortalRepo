using CBSEWebPortal.Models;
using CBSEWebPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace CBSEWebPortal.Services
{
    public class CBSEAdminService : ICBSEAdminService
    {
        private readonly AppDbContext _context;

        public CBSEAdminService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAdminAsync(CBSEAdmin admin)
        {
            try
            {
                Console.WriteLine($"Email: {admin.Email}, Name: {admin.Name}, Password: {admin.Password}");
                admin.Password = BCrypt.Net.BCrypt.HashPassword(admin.Password);
                _context.CBSEAdmin.Add(admin);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding admin: {ex.Message}");
                return false;
            }
        }

        public async Task<List<StudentRequest>> GetPendingStudentRequestsAsync()
        {
            Console.WriteLine("Hello from studentrequest");
            return await _context.StudentRequests
                .Where(sr => sr.Status == "Pending")
                .ToListAsync();
        }

        public async Task<StudentRequest> EnrollStudentAsync(StudentRequest request)
        {
            try
            {
                var existingRequest = await _context.StudentRequests.FirstOrDefaultAsync(sr => sr.Id == request.Id);
                var principal = await _context.Principal
                    .Include(p => p.School)
                    .FirstOrDefaultAsync(p => p.ID == request.PrincipalId);

                if (principal == null)
                {
                    return null;
                }

                var school = principal.School;
                if (school == null)
                {
                    return null;
                }

                var rollNumber = $"{school.Name[0].ToString().ToUpper()}{new Random().Next(1000, 9999)}";
                var newStudent = new Student
                {
                    Name = request.StudentName,
                    Roll_No = rollNumber,
                    School = school
                };

                _context.Student.Add(newStudent);
                await _context.SaveChangesAsync();

                existingRequest.Status = "Enrolled";
                _context.StudentRequests.Update(existingRequest);
                await _context.SaveChangesAsync();
                return existingRequest;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CBSEAdmin> GetAdminByEmailAsync(string email)
        {
            return await _context.CBSEAdmin.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<bool> ValidateAdminCredentialsAsync(string email, string password)
        {
            var admin = await GetAdminByEmailAsync(email);
            return admin != null && BCrypt.Net.BCrypt.Verify(password, admin.Password);
        }


        public async Task<IEnumerable<School>> GetAllSchoolsAsync()
        {
            return await _context.School.Include(s => s.Principal).ToListAsync();
        }

        public async Task<SubjectMarksDto> GetStudentMarksAsync(int studentId)
        {
            var student = await _context.Student.Include(s => s.SubjectMarks)
       .FirstOrDefaultAsync(s => s.ID == studentId);

            if (student == null)
                return null;

            var marksDto = new SubjectMarksDto
            {
                Sub1 = student.SubjectMarks?.Sub1 ?? 0,  //null-coalescing operator to avoid null reference exception
                Sub2 = student.SubjectMarks?.Sub2 ?? 0,
                Sub3 = student.SubjectMarks?.Sub3 ?? 0,
                Sub4 = student.SubjectMarks?.Sub4 ?? 0,
                Sub5 = student.SubjectMarks?.Sub5 ?? 0
            };

            return marksDto;
        }
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Student.Include(s => s.School).ToListAsync();
        }

        public async Task<SubjectMarks> UpdateSubjectMarksAsync(int studentId, SubjectMarksDto marksDto)
        {
            var student = await _context.Student
                .Include(s => s.SubjectMarks)
                .FirstOrDefaultAsync(s => s.ID == studentId);

            if (student == null)
                return null;

            if (student.SubjectMarks == null)
            {
                var marks = new SubjectMarks
                {
                    StudentID = studentId,
                    Sub1 = marksDto.Sub1,
                    Sub2 = marksDto.Sub2,
                    Sub3 = marksDto.Sub3,
                    Sub4 = marksDto.Sub4,
                    Sub5 = marksDto.Sub5
                };

                _context.SubjectMarks.Add(marks);
                await _context.SaveChangesAsync();
                return marks;
            }
            else
            {
                student.SubjectMarks.Sub1 = marksDto.Sub1;
                student.SubjectMarks.Sub2 = marksDto.Sub2;
                student.SubjectMarks.Sub3 = marksDto.Sub3;
                student.SubjectMarks.Sub4 = marksDto.Sub4;
                student.SubjectMarks.Sub5 = marksDto.Sub5;

                _context.SubjectMarks.Update(student.SubjectMarks);
                await _context.SaveChangesAsync();
                return student.SubjectMarks;
            }

        }

    }
}
