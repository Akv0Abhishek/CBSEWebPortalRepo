using System.ComponentModel.DataAnnotations;

namespace CBSEWebPortal.Models
{
    public class CBSEAdmin
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
    }
}
