using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;
using Instagram.ViewModels;
using Instagram.Globals;
namespace Instagram.Controllers
{
    public class SearchController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Search

        public ActionResult Search(string Searching)
        {
            int id = GlobalUserID.get();
            ViewData["ID"] = id;
            //user can search by first name-last name-email
            return View(db.users.Where(x => x.Fname.StartsWith(Searching) || x.Lname.StartsWith(Searching) || Searching == null||x.email.StartsWith(Searching)).ToList());
        }



    }
}