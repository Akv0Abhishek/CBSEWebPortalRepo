using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBSEWebPortal.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Roll_No { get; set; }

        [ForeignKey("School")]
        public int SchoolID { get; set; }
        public School School { get; set; }

        public SubjectMarks SubjectMarks { get; set; }
    }
}
