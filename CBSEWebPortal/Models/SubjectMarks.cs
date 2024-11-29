using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBSEWebPortal.Models
{
    public class SubjectMarks
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Student")]
        public int StudentID { get; set; }
        public Student Student { get; set; }

        [Required]
        public int Sub1 { get; set; }

        [Required]
        public int Sub2 { get; set; }

        [Required]
        public int Sub3 { get; set; }

        [Required]
        public int Sub4 { get; set; }

        [Required]
        public int Sub5 { get; set; }
    }
}
