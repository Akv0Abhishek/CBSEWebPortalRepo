using Microsoft.EntityFrameworkCore;
using CBSEWebPortal.Models;

namespace CBSEWebPortal.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Principal> Principal { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<CBSEAdmin> CBSEAdmin { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentRequest> StudentRequests { get; set; }
        public DbSet<SubjectMarks> SubjectMarks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Principal to School Relationship
            modelBuilder.Entity<Principal>()
                .HasOne(p => p.School)
                .WithMany(s => s.Principal)
                .HasForeignKey(p => p.SchoolID)
                .OnDelete(DeleteBehavior.Cascade);

            // School to Students Relationship
            modelBuilder.Entity<School>()
                .HasMany(s => s.Students)
                .WithOne(st => st.School)
                .HasForeignKey(st => st.SchoolID)
                .OnDelete(DeleteBehavior.Cascade);

            // Student to SubjectMarks Relationship
            modelBuilder.Entity<Student>()
                .HasOne(st => st.SubjectMarks)
                .WithOne(sm => sm.Student)
                .HasForeignKey<SubjectMarks>(sm => sm.StudentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CBSEAdmin>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(255).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
