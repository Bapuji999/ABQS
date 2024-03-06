using System.ComponentModel.DataAnnotations;

namespace ABQStask.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public string ImagePath { get; set; }
        public bool isDeleted { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
