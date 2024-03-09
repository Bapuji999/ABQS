using ABQStask.Models;
using System.ComponentModel.DataAnnotations;

namespace ABQStask.CommandModel
{
    public class AddProductCommandModel
    {
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile ProductImage { get; set; }

        public List<Category> Categories { get; set; }
    }
}
