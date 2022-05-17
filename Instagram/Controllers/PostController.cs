using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;
using Instagram.Globals;
using System.Data.Entity.Validation;
using System.IO;


namespace Instagram.Controllers
{
    public class PostController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Post
        [HttpGet]
        public ActionResult CreatePost()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreatePost(post post)
        {
 
            if (ModelState.IsValid)
            {
                post.OwnerID = GlobalUserID.get();
                string FileName = Path.GetFileNameWithoutExtension(post.ImageFile.FileName);
                string Extension = Path.GetExtension(post.ImageFile.FileName);
                FileName = FileName + DateTime.Now.ToString("yymmssffff") + Extension;
                post.ImagePath = "~/Image/" + FileName;
                FileName = Path.Combine(Server.MapPath("~/Image/"), FileName);
                post.ImageFile.SaveAs(FileName);
                db.posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("ViewAllPosts", "Home");
            }
            return View();
        }


    }
}