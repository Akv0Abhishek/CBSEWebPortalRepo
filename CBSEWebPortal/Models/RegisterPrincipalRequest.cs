using System.ComponentModel.DataAnnotations;

namespace CBSEWebPortal.Models
{
    public class RegisterPrincipalRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        public string SchoolName { get; set; }
    }
}