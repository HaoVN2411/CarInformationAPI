using NuGet.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarCategoriesApi.Data
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        [Key]
        public int TokenID { get; set; }
        public int userID { get; set; }
        [ForeignKey(nameof(userID))]
        public UserLogin UserLogin { get; set; }

        public string token { get; set; }
        public string JwtId { get; set; }
        public bool isUsed { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ExpireTime { get; set;}

    }
}
