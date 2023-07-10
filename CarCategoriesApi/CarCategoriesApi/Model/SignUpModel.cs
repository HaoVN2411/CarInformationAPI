using System.ComponentModel.DataAnnotations;

namespace CarCategoriesApi.Models
{
    public class SignUpModel
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(100)]
        public string Passwpord { get; set; }
        [Required]
        [MaxLength(100)]
        public string ConfirmPasswpord { get; set; }
        [Required]
        [MaxLength(100)]
        public String FullName { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public String Email { get; set; } 
    }
}
