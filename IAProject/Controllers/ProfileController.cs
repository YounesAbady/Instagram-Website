using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;
using Instagram.Globals;
using System.Data.Entity;
using System.IO;


namespace Instagram.Controllers
{
    public class ProfileController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Profile
        public ActionResult ViewProfile()
        {   

            var user=db.users.Single(x => x.UserID==GlobalUserID.globalUserID); 
            if (user==null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpGet]
        public ActionResult EditProfile(int id)
        {
            var user = db.users.Single(x => x.UserID ==id);
            if (user == null)
                return HttpNotFound();
            else
            {
                return View(user);
            }
            
        }
        [HttpPost]
        public ActionResult EditProfile(user u)
        {
            if (ModelState.IsValid)
            {
                var user = db.users.Single(x => x.UserID ==u.UserID);
                user.Fname = u.Fname;
                user.Lname = u.Lname;
                user.Password = u.Password;
                user.mobile = u.mobile;
                user.email = u.email;
                if (u.ImagePath!= null)
                { 
                string FileName = Path.GetFileNameWithoutExtension(u.ImageFile.FileName);
                string Extension = Path.GetExtension(u.ImageFile.FileName);
                FileName = FileName + DateTime.Now.ToString("yymmssffff") + Extension;
                u.ImagePath = "~/Image/" + FileName;
                user.ImagePath = u.ImagePath;
                FileName = Path.Combine(Server.MapPath("~/Image/"), FileName);
                u.ImageFile.SaveAs(FileName);
                }  

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewProfile");
            }
            return View();

        }
    }
}