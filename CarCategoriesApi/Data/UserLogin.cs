using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarCategoriesApi.Data
{
    [Table("UserLogin")]
    public class UserLogin
    {
        [Key]
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
