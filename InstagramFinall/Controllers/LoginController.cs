using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstagramFinall.Models;
using InstagramFinall.Globals;
namespace Instagram.Controllers
{
    public class LoginController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User log)
        {
            User user = db.Users.SingleOrDefault(x => x.Email == log.Email && x.Password == log.Password);
            if (user != null)
            {
                GlobalUserId.Instance(user.UserId);
                return RedirectToAction("ViewAllPosts", "Home");
            }
            else { return RedirectToAction("Login"); }
        }
    }
}