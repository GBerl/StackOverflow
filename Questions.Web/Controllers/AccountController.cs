using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Questions.Data;

namespace Questions.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connection;
        public AccountController(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("Con");
        }
        public IActionResult Login()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Message = TempData["Error"];
                ViewBag.Color = "red";
            }

            return View();
        }
        [HttpPost]
        public IActionResult Login(string password, string email)
        {
            var repository = new AccountRepository(_connection);
            var user = repository.Login(password, email);
            if (user == null)
            {
                TempData["Error"] = "Invalid Login Please Try Again";
                return Redirect("/account/login");
            }
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/askquestion");
        }
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAccount(string password, string email, string name)
        {
            var repository = new AccountRepository(_connection);
            repository.AddUser(name, email, password);
            return Redirect("/account/login");
        }
        [Authorize]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/account/login");
        }
    }
}