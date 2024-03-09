using System.ComponentModel.DataAnnotations;

namespace ABQStask.CommandModel
{
	public class CategoryComandModel
	{
		[Required(ErrorMessage = "Category name is required")]
		public string CategoryName { get; set; }

		[Required(ErrorMessage = "Category image is required")]
		public IFormFile CategoryImage { get; set; }
	}
}
