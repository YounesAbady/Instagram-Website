using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAProject.Models;


namespace IAProject.Controllers
{
    public class PostController : Controller
    {
        ProjectDataBase db = new ProjectDataBase();
        // GET: Post
        [HttpGet]
        public ActionResult CreatePost(user u)
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreatePost(post post, user u)
        {
            post.OwnerID = u.UserID;
            if (ModelState.IsValid)
            {
                db.posts.Add(post);
                db.SaveChanges();
                return View();
            }
            return View();
        }


    }
}