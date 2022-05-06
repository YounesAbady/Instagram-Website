using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;

namespace Instagram.Controllers
{
    public class SearchController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string Searching)
        {
            return View(db.users.Where(x => x.Fname.StartsWith(Searching) || x.Lname.StartsWith(Searching) || Searching == null).ToList());
        }
    }
}