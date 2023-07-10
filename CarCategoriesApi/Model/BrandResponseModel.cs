using CarCategoriesApi.Data;
using System.ComponentModel.DataAnnotations;

namespace CarCategoriesApi.Models
{
    public class BrandResponseModel
    {
        public int BrandId { get; set; }
        [MaxLength(100)]
        public string BrandName { get; set; }
        public string BrandDescription { get; set; }
    }
}
