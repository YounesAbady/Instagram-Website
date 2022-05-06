using Instagram.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Globals;
namespace Instagram.Controllers
{
    public class LoginOrRegistrationController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: LoginOrRegistration
        [HttpGet]
        public ActionResult Registration()
        {
            user user = new user();
            return View(user);
        }

        [HttpPost]
        public ActionResult Registration(user user)
        {
            if (!ModelState.IsValid)
            {
                return View("Registration", user);
            }
            string FileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
            string Extension = Path.GetExtension(user.ImageFile.FileName);
            FileName = FileName + DateTime.Now.ToString("yymmssffff") + Extension;
            user.ImagePath = "~/Image/" + FileName;
            FileName = Path.Combine(Server.MapPath("~/Image/"), FileName);
            user.ImageFile.SaveAs(FileName);
            using (ProjectDataBaseEntities db = new ProjectDataBaseEntities())
            {
                db.users.Add(user);
                db.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("Login");
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(user log)
        {
            user user = db.users.Single(x => x.email == log.email && x.Password == log.Password);
            if (user!=null)
            {

                GlobalUserID.globalUserID = user.UserID;
                return RedirectToAction("CreatePost", "Post");
            }
            else { return RedirectToAction("Login"); }
        }

    }
}