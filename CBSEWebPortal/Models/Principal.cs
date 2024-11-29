using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBSEWebPortal.Models
{
    public class Principal
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [ForeignKey("School")]
        public int? SchoolID { get; set; }
        public School School { get; set; }

    }
}
