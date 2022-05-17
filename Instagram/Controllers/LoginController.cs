using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;
using Instagram.Globals;
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
        public ActionResult Login(user log)
        {
            user user = db.users.SingleOrDefault(x => x.email == log.email && x.Password == log.Password);
            if (user != null)
            {
                GlobalUserID.Instance(user.UserID);
                return RedirectToAction("ViewAllPosts", "Home");
            }
            else { return RedirectToAction("Login"); }
        }
    }
}