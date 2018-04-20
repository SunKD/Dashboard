using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private DashboardContext _context;

        public HomeController(DashboardContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("CurrentUserID") != null)
            {
                ViewBag.Loggedin = (int)HttpContext.Session.GetInt32("CurrentUserID");
            }
            return View();
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [Route("processregister")]
        public IActionResult ProcessRegister(RegisterViewModel model)
        {
            int level = (int)_context.Users.ToList().Count == 0 ? 9 : 1;
            User admin = _context.Users.Where(e =>e.UserID == (int)HttpContext.Session.GetInt32("CurrentUserID")).SingleOrDefault();


            if (ModelState.IsValid)
            {
                User NewUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserLevel = level,
                    Password = model.Password
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);

                _context.Users.Add(NewUser);
                _context.SaveChanges();
                
                HttpContext.Session.SetInt32("CurrentUserID", NewUser.UserID);
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Something went wrong..";
            return View("Register");
        }


        [HttpPost]
        [Route("loggingIn")]
        public IActionResult LoggingIn(string loginemail, string loginpw)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();

            var loginUser = _context.Users.SingleOrDefault(User => User.Email == loginemail);
            if (loginUser != null)
            {
                var hashedPw = Hasher.VerifyHashedPassword(loginUser, loginUser.Password, loginpw);
                if (hashedPw == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("CurrentUserID", loginUser.UserID);
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Error = "Email address or Password is not matching";
            return View("Login");
        }


        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
