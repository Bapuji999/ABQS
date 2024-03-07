using ABQS.Models.CommandModel;
using ABQStask.Data;
using ABQStask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        [AllowAnonymous]
        public IActionResult Resister()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Resister(LoginCommandModel loginCommandModel)
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginCommandModel loginCommandModel)
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
                    var token = Generate(user);
                    HttpContext.Session.SetString("Token", token);
                    return RedirectToAction("Index", "Home");
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
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Name),
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
