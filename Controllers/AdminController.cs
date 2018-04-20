using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class AdminController : Controller
    {
        private DashboardContext _context;

        public AdminController(DashboardContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            if(!CheckLoggedIn()){
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Loggedin = (int)HttpContext.Session.GetInt32("CurrentUserID");
            var allUsers = _context.Users.ToList();
            return View("AdminDashboard", allUsers);
        }

        [HttpGet]
        [Route("addNew")]
        public IActionResult AddNewUser(){
            User admin = _context.Users.Where(predicate: e =>e.UserID == (int)HttpContext.Session.GetInt32("CurrentUserID")).SingleOrDefault();
            if(!CheckLoggedIn() || admin.UserLevel != 9){
                return RedirectToAction("Index", "Home");
            }
            return View("AddNew");
        }

        [HttpPost]
        [Route("processAdd")]
        public IActionResult ProcessAdd(RegisterViewModel model){
            if (ModelState.IsValid)
            {
                User NewUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserLevel = 1,
                    Password = model.Password
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);

                _context.Users.Add(NewUser);
                _context.SaveChanges();
                
                return RedirectToAction("Dashboard");
            }
            ViewBag.Loggedin = (int)HttpContext.Session.GetInt32("CurrentUserID");
            ViewBag.Error = "Something went wrong..";
            return View("AddNew");
        }

        [HttpPost]
        [Route("modify")]
        public IActionResult Modify(int userID, string modify){
            if(!CheckLoggedIn()){
                return RedirectToAction("Index", "Home");
            }
            
            if(modify == "Edit"){
                User modifyUser = _context.Users.Where(e=>e.UserID == userID).SingleOrDefault();
                ViewBag.Loggedin = (int)HttpContext.Session.GetInt32("CurrentUserID");
                return View("EditUser", modifyUser);
            }else{
                var delete = _context.Users.Where(e=>e.UserID == userID).SingleOrDefault();
                _context.Users.Remove(delete);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Route("editinfo")]
        public IActionResult EditInfo(EditViewModel model, int UserID, string UserPW, int UserLevel){
            if(!CheckLoggedIn()){
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var editUser = _context.Users.Where(e=>e.UserID == UserID).SingleOrDefault();
                    editUser.FirstName = model.FirstName;
                    editUser.LastName = model.LastName;
                    editUser.Email = model.Email;
                    editUser.Password = UserPW;
                    editUser.UserLevel = UserLevel;


                _context.Users.Update(editUser);
                _context.SaveChanges();
                ViewBag.Loggedin = (int)HttpContext.Session.GetInt32("CurrentUserID");
                return View("Edit");
            }
            ViewBag.Error = "Something went wrong..";
            return View("Modify");
        }

        [HttpPost]
        [Route("editpw")]
        public IActionResult EditPw(PwEditViewModel model, int UserID, string FirstName, string LastName, int UserLevel, string UserEmail){
            if(!CheckLoggedIn()){
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var editUser = _context.Users.Where(e=>e.UserID == UserID).SingleOrDefault();
                    editUser.Password = model.Password;

                _context.Users.Update(editUser);
                _context.SaveChanges();
                ViewBag.Loggedin = (int)HttpContext.Session.GetInt32("CurrentUserID");
                return RedirectToAction("Edit");
            }
            ViewBag.Error = "Something went wrong..";
            return View("Modify");
        }

        [HttpGet]
        [Route("show/{UserID}")]
        public IActionResult Show(int UserID){
            if(!CheckLoggedIn()){
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.Loggedin = (int)HttpContext.Session.GetInt32("CurrentUserID");
            var query = _context.Users.Include(e => e.Recieved).ThenInclude(r =>r.Comments).Where(u=>u.UserID == UserID).ToList();
            return View("Show", query);
        }

        [HttpPost]
        [Route("show/postMessage")]
        public IActionResult PostMessage(int reciever, string msg){
            if(!CheckLoggedIn()){
                return RedirectToAction("Index", "Home");
            }

            Message newMessage = new Message{
                Msg = msg,
                WriterID = (int)HttpContext.Session.GetInt32("CurrentUserID"),
                ReceiverID = reciever
            };
            _context.Messages.Add(newMessage);
            _context.SaveChanges();
            return RedirectToAction("Show", "Admin", new{UserID = reciever});
        }

        [HttpPost]
        [Route("show/postComment")]
        public IActionResult PostComment(int msgID, string comment, int originalUserID){
            if(!CheckLoggedIn()){
                return RedirectToAction("Index", "Home");
            }

            Comment newComment = new Comment{
                Cmt = comment,
                UserID = (int)HttpContext.Session.GetInt32("CurrentUserID"),
                MessageID = msgID
            };
            _context.Comments.Add(newComment);
            _context.SaveChanges();
            return RedirectToAction("Show", "Admin", new{UserID = originalUserID});
        }

        public bool CheckLoggedIn()
        {
            if (HttpContext.Session.GetInt32("CurrentUserID") == null)
            {
                return false;
            }
            return true;
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
