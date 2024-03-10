using ABQStask.CommandModel;
using ABQStask.Data;
using ABQStask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ABQStask.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AbqsContex _dbContext;

        public AuthController(IConfiguration configuration, AbqsContex dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
            return Ok(new { success = true });
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Resister()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Resister(UserRegistrationModel userRegistrationModel)
        {
            if (ModelState.IsValid)
            {
                if(_dbContext.Users.Any(x => x.Email == userRegistrationModel.Email))
                {
                    ModelState.AddModelError("", "EmailId already present please try with diffrent emailid.");
                    return View(userRegistrationModel);
                }
                User user = new User();
                user.Email = userRegistrationModel.Email;
                user.UserId = new Guid();
                user.RoleId = 3;
                user.Name = userRegistrationModel.UserName;
                user.isDeleted = false;
                user.Phone = userRegistrationModel.PhoneNumber;
                user.Password = userRegistrationModel.Password;
                var token = "Bearer " + Generate(user);
                HttpContext.Session.SetString("Token", token);
                var roleName = _dbContext.Roles.Where(x => x.RoleId == user.RoleId).FirstOrDefault()?.RollName;
                HttpContext.Session.Remove("Role");
                HttpContext.Session.SetString("Role", roleName);
                _dbContext.Add(user);
                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Users Resistered successfully."; 
                TempData["Token"] = token;
                TempData["Role"] = roleName;
                return RedirectToAction("Index", "Home");
            }
            return View(userRegistrationModel);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginCommandModel loginCommandModel, string gRecaptchaResponse)
        {
            try
            {
                if (HttpContext.Session.GetString("Token") != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var user = Authenticate(loginCommandModel);

                if (user != null)
                {
                    var token = "Bearer " + Generate(user);
                    HttpContext.Session.SetString("Token", token);
                    var roleName = _dbContext.Roles.Where(x => x.RoleId == user.RoleId).FirstOrDefault()?.RollName;
                    HttpContext.Session.Remove("Role");
                    HttpContext.Session.SetString("Role", roleName);
                    TempData["SuccessMessage"] = "Login successfull.";
                    return Ok(new { token, roleName });
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(loginCommandModel);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest("An error occurred while processing your request.");
            }
        }
        
        private User Authenticate(LoginCommandModel loginCommandModel)
        {
            try
            {
                var user = _dbContext.Users.Where(x => x.Email == loginCommandModel.EmailID && x.Password == loginCommandModel.Password && !x.isDeleted).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string Generate(User user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var roleName = _dbContext.Roles.Where(x => x.RoleId == user.RoleId).FirstOrDefault()?.RollName;
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Name),
                    new Claim(ClaimTypes.Role, roleName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone, user.Phone)
                };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
