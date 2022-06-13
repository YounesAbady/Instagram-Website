using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstagramFinall.Models;
using InstagramFinall.Globals;
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
        public ActionResult CreatePost(Post Post)
        {

            if (ModelState.IsValid)
            {
                Post.OwnerId = GlobalUserId.Get();
                string FileName = Path.GetFileNameWithoutExtension(Post.ImageFile.FileName);
                string Extension = Path.GetExtension(Post.ImageFile.FileName);
                FileName = FileName + DateTime.Now.ToString("yymmssffff") + Extension;
                Post.ImagePath = "~/Image/" + FileName;
                FileName = Path.Combine(Server.MapPath("~/Image/"), FileName);
                Post.ImageFile.SaveAs(FileName);
                db.Posts.Add(Post);
                db.SaveChanges();
                return RedirectToAction("ViewAllPosts", "Home");
            }
            return View();
        }


    }
}