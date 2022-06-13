using InstagramFinall.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstagramFinall.Globals;
namespace InstagramFinall.Controllers
{
    public class RegistrationController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Registration
        [HttpGet]
        public ActionResult Registration()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public ActionResult Registration(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("Registration", user);
            }
            string fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
            string extension = Path.GetExtension(user.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssffff") + extension;
            user.ImagePath = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            user.ImageFile.SaveAs(fileName);
            using (ProjectDataBaseEntities db = new ProjectDataBaseEntities())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("Login", "Login");
        }




    }
}