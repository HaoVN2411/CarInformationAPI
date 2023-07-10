using System.ComponentModel.DataAnnotations;

namespace CarCategoriesApi.Models
{
    public class CarResponseModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string BrandName { get; set; }
        public string? Description { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
    }
}
