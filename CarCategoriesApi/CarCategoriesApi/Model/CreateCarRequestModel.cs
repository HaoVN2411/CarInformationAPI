using System.ComponentModel.DataAnnotations;

namespace CarCategoriesApi.Models
{
    public class CreateCarRequestModel
    {
        public string Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        public int BrandId { get; set; }
    }
}
