using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;
using Instagram.ViewModels;
namespace Instagram.Controllers
{
    public class HomeController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Home
        public ActionResult ViewAllPosts()
        {
            FullHomePage home = new FullHomePage
            {
                User = db.users.ToList(),
                posts=db.posts.ToList()
            };
            
            return View(home);
        }
    }
}