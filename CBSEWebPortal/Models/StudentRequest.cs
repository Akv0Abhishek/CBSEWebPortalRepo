//using System.ComponentModel.DataAnnotations.Schema;
namespace CBSEWebPortal.Models
{
    public class StudentRequest
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public int PrincipalId { get; set; }

        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}