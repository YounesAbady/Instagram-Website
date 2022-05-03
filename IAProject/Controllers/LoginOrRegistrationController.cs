using IAProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAProject.Controllers
{
    public class LoginOrRegistrationController : Controller
    {
        ProjectDataBase db = new ProjectDataBase();
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
            using (ProjectDataBase db = new ProjectDataBase())
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
            var user = db.users.Where(x => x.email == log.email && x.Password == log.Password).Count();
            if (user > 0) { return RedirectToAction("CreatePost","Post",new {userr =user }); }
            else { return RedirectToAction("Login"); }
        }

    }
}