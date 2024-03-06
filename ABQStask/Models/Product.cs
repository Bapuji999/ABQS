using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABQStask.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public bool isDeleted { get; set; }
        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
    }
}
