using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarCategoriesApi.Data
{
    [Table("CarInfo")]
    public class CarInfo
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        public int? BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public BrandInfo BrandInfo { get; set; }
    }
}
