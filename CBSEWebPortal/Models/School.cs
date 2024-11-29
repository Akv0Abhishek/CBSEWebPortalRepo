using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CBSEWebPortal.Models
{
    public class School
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<Principal> Principal { get; set; }
    }
}
