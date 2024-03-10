using ABQStask.Data;
using ABQStask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABQStask.Controllers
{
    [Authorize(Roles ="User")]
    public class UserController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly AbqsContex _dbContext;
        public UserController(IConfiguration configuration, AbqsContex dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Products()
        {
            var products = await _dbContext.Products.Include(p => p.Category).Where(p => !p.isDeleted).ToListAsync(); 
            foreach (var product in products)
            {
                product.ImagePath = "/"+product.ImagePath.Replace("wwwroot\\", "");
            }
            return View(products);
        }
        public async Task<IActionResult> Categories()
        {
            var categories = await _dbContext.Categories.Where(c => !c.isDeleted).ToListAsync();
            foreach (var category in categories)
            {
                category.ImagePath = "/" + category.ImagePath.Replace("wwwroot\\", "");
            }
            return View(categories);
        }
    }
}
