using ABQStask.CommandModel;
using ABQStask.Data;
using ABQStask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABQStask.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AbqsContex _dbContext;
        public AdminController(IConfiguration configuration, AbqsContex dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryComandModel categoryComandModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var imagePath = Path.Combine("wwwroot", "images", Guid.NewGuid().ToString() + categoryComandModel.CategoryImage.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await categoryComandModel.CategoryImage.CopyToAsync(stream);
                    }

                    var category = new Category
                    {
                        CategoryId = Guid.NewGuid(),
                        CategoryName = categoryComandModel.CategoryName,
                        isDeleted = false,
                        ImagePath = imagePath
                    };
                    _dbContext.Categories.Add(category);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                return View(categoryComandModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            var viewModel = new AddProductCommandModel
            {
                Categories = _dbContext.Categories.ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductCommandModel addProductCommandModel)
        {
            try
            {
                ModelState.Remove("Categories");
                if (ModelState.IsValid)
                {
                    var imagePath = Path.Combine("wwwroot", "images", Guid.NewGuid().ToString() + addProductCommandModel.ProductImage.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await addProductCommandModel.ProductImage.CopyToAsync(stream);
                    }
                    var product = new Product
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = addProductCommandModel.ProductName,
                        CategoryId = addProductCommandModel.CategoryId,
                        Price = addProductCommandModel.Price,
                        ImagePath = imagePath,
                        isDeleted = false
                    };

                    _dbContext.Products.Add(product);
                    await _dbContext.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Product added successfully!";
                    return RedirectToAction("Index", "Home");
                }
                addProductCommandModel.Categories = _dbContext.Categories.ToList();
                return View(addProductCommandModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public IActionResult DeleteUser()
        {
            try
            {
                var users = _dbContext.Users.Where(x => x.isDeleted == false && x.RoleId == 3).ToList();
                var userDeleteCommandModel = users.Select(u => new UserDeleteCommandModel
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    IsSelected = false
                }).ToList();

                return View(userDeleteCommandModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(List<UserDeleteCommandModel> userDeleteCommandModel)
        {
            try
            {
                var selectedUsers = userDeleteCommandModel.Where(u => u.IsSelected).ToList();
                if (selectedUsers.Count > 0)
                {
                    foreach (var user in selectedUsers)
                    {
                        var userToDelete = _dbContext.Users.Where(u => u.UserId == user.UserId).FirstOrDefault();
                        if (userToDelete != null)
                        {
                            userToDelete.isDeleted = true;
                            _dbContext.Update(userToDelete);
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Users deleted successfully.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Please select at least one user to delete.");
                }
                var users = _dbContext.Users.Where(x => x.isDeleted == false).ToList();
                var usersReturn = users.Select(u => new UserDeleteCommandModel
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    IsSelected = false
                }).ToList();

                return View(usersReturn);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
