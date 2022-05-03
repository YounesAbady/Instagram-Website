using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAProject.Models;

namespace IAProject.Controllers
{
    public class HomeController : Controller
    {
        ProjectDataBase db = new ProjectDataBase();
        public ActionResult Index()
        {
            List<user> users = db.users.ToList();
            return View(users);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}