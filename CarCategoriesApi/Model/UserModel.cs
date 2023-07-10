using System.ComponentModel.DataAnnotations;

namespace CarCategoriesApi.Models
{
    public class UserModel
    {
        public int id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(100)]
        public string Passwpord { get; set; }
        public String Role { get; set; }
        [Required]
        [MaxLength(100)]
        public String FullName { get; set; }
        [Required]
        [MaxLength(100)]
        public String Email { get; set; }
    }
}
