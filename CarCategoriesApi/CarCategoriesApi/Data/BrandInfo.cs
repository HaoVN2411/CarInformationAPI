using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarCategoriesApi.Data
{
    [Table("BrandInfo")]
    public class BrandInfo
    {
        [Key]
        public int BrandId { get; set; }
        [MaxLength(100)]
        public string BrandName { get; set; }
        public string BrandDescription { get; set; }
        public virtual ICollection<CarInfo> Brands { get; set; }
    }
}
